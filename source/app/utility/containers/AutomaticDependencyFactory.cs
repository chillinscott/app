using System.Reflection;

namespace app.utility.containers
{
  public class AutomaticDependencyFactory<TType> : ICreateADependencyInstance
  {
      IDetermineWhichConstructorShouldBeUsedToCreateAType ctor_picker;
      IFetchDependencies container;

      public AutomaticDependencyFactory(IDetermineWhichConstructorShouldBeUsedToCreateAType constructor_selection_specification, IFetchDependencies container)
      {
          this.ctor_picker = constructor_selection_specification;
          this.container = container;
      }

      public object create()
      {
         ConstructorInfo constructor = ctor_picker.get_the_ctor_to_create_the(typeof(TType));
         ParameterInfo[] parameters = constructor.GetParameters();
         var constructor_parameters = new object[parameters.Length];
          for (int i = 0; i < parameters.Length; i++)
          {
              var parameter_info = parameters[i];
              constructor_parameters[i] = container.an(parameter_info.ParameterType);
          }
          
          return constructor.Invoke(BindingFlags.CreateInstance, constructor_parameters);

      }
  }
}