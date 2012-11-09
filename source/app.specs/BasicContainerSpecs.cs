using System;
using System.Data;
using Machine.Specifications;
using app.utility.containers;
using developwithpassion.specifications.extensions;
using developwithpassion.specifications.rhinomocks;

namespace app.specs
{
  [Subject(typeof(BasicContainer))]
  public class BasicContainerSpecs
  {
    public abstract class concern : Observes<IFetchDependencies,
                                      BasicContainer>
    {
    }

    public class when_fetching_a_dependency : concern
    {
      public class at_runtime
      {
        Establish c = () =>
        {

          factories = depends.on<IFindDependencyFactories>();
          the_connection = fake.an<IDbConnection>();
          factory = fake.an<ICreateOneDependency>();

          factory.setup(x => x.create()).Return(the_connection);
          factories.setup(x => x.get_the_factory_that_can_create(typeof(IDbConnection))).Return(factory);
        };

        Because b = () =>
          result = sut.an(typeof(IDbConnection));

        It should_return_the_item_created_by_the_factory_that_can_create_that_dependency = () =>
          result.ShouldEqual(the_connection);

        static IFindDependencyFactories factories;
        static ICreateOneDependency factory;
        static object result;
        static IDbConnection the_connection;
      }

      public class and_it_has_the_factory_that_can_create_the_dependency
      {
        Establish c = () =>
        {
          factories = depends.on<IFindDependencyFactories>();
          the_connection = fake.an<IDbConnection>();
          factory = fake.an<ICreateOneDependency>();

          factory.setup(x => x.create()).Return(the_connection);

          factories.setup(x => x.get_the_factory_that_can_create(typeof(IDbConnection))).Return(factory);
        };

        Because b = () =>
          result = sut.an<IDbConnection>();

        It should_return_the_item_created_by_the_factory_that_can_create_that_dependency = () =>
          result.ShouldEqual(the_connection);

        static IFindDependencyFactories factories;
        static ICreateOneDependency factory;
        static IDbConnection result;
        static IDbConnection the_connection;
      }

      public class and_the_factory_that_can_create_the_dependency_throws_an_error_while_creating_it
      {
        Establish c = () =>
        {
          factories = depends.on<IFindDependencyFactories>();
          factory = fake.an<ICreateOneDependency>();
          inner_exception = new Exception();
          the_custom_exception = new Exception();

          depends.on<ICreateTheExceptionWhenAnDependencyFactoryCantCreateItsItem>((type, inner) =>
          {
            type.ShouldEqual(typeof(IDbConnection));
            inner.ShouldEqual(inner_exception);
            return the_custom_exception;
          });

          factory.setup(x => x.create()).Throw(inner_exception);
          factories.setup(x => x.get_the_factory_that_can_create(typeof(IDbConnection))).Return(factory);
        };

        Because b = () =>
          spec.catch_exception(() => sut.an<IDbConnection>());

        It should_return_the_item_created_by_the_factory_that_can_create_that_dependency = () =>
          spec.exception_thrown.ShouldEqual(the_custom_exception);

        static IFindDependencyFactories factories;
        static ICreateOneDependency factory;
        static Exception the_custom_exception;
        static Exception inner_exception;
      }
    }
  }
}