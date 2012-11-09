using System.Data;
using System.Reflection;
using Machine.Specifications;
using app.specs.utility;
using app.utility.containers;
using developwithpassion.specifications.extensions;
using developwithpassion.specifications.rhinomocks;

namespace app.specs
{
  [Subject(typeof(AutomaticDependencyFactory<>))]
  public class AutomaticDependencyFactorySpecs
  {
    public abstract class concern : Observes<ICreateADependencyInstance,
                                      AutomaticDependencyFactory<ItemWithItems>>
    {
    }

    public class when_creating_its_item : concern
    {
      Establish c = () =>
      {
        connection = fake.an<IDbConnection>();
        other = new Other();
        command = fake.an<IDbCommand>();
        container = depends.on<IFetchDependencies>();
        depends.on(typeof(ItemWithItems));
        ctor_picker = depends.on<IDetermineWhichConstructorShouldBeUsedToCreateAType>();

        greediest =
          ObjectFactory.expressions.to_target<ItemWithItems>().ctor_pointed_at_by(
            () => new ItemWithItems(null, null, null));


        ctor_picker.setup(x => x.get_the_ctor_to_create_the(typeof(ItemWithItems))).Return(greediest);
        container.setup(x => x.an(typeof(IDbConnection))).Return(connection);
        container.setup(x => x.an(typeof(IDbCommand))).Return(command);
        container.setup(x => x.an(typeof(Other))).Return(other);
      };

      Because b = () =>
        result = sut.create();

      It should_return_the_item_created_with_all_of_its_dependencies_satisfied = () =>
      {
        var item = result.ShouldBeAn<ItemWithItems>();
        item.connection.ShouldEqual(connection);
        item.command.ShouldEqual(command);
        item.other.ShouldEqual(other);
      };

      static object result;
      static IDbCommand command;
      static Other other;
      static IDbConnection connection;
      static IFetchDependencies container;
      static IDetermineWhichConstructorShouldBeUsedToCreateAType ctor_picker;
      static ConstructorInfo greediest;
    }

    public class ItemWithItems
    {
      public IDbConnection connection;
      public IDbCommand command;
      public Other other;

      public ItemWithItems(IDbConnection connection, IDbCommand command, Other other)
      {
        this.connection = connection;
        this.command = command;
        this.other = other;
      }

      public ItemWithItems(IDbConnection connection, IDbCommand command)
      {
        this.connection = connection;
        this.command = command;
      }
    }

    public class Other
    {
    }
  }
}