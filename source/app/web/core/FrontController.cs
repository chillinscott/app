﻿namespace app.web.core
{
  public class FrontController : IProcessRequests
  {
      readonly IFindCommands command_registry;

      public FrontController(IFindCommands command_registry)
      {
          this.command_registry = command_registry;
      }

      public void process(IContainRequestDetails request)
      {
          command_registry.get_the_command_that_can_run(request).run(request);
      }
  }
}