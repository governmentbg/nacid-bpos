export enum DebugVerbosity {
  methodInvocation = 1,
  methodVerbose = 2
}

interface IDebugLogger {
  logFactory(information);
  logDecoration(information);
  logBeforeMethod(methodName: string, args: any);
  logAfterMethod(methodName: string, result: any);
}

class MethodInvocationDebugLogger implements IDebugLogger {
  logFactory(information: any) { }

  logDecoration(information: any) { }

  logBeforeMethod(methodName: string, args: any) {
    console.log(`DEBUG: calling "${methodName}"`);
  }

  logAfterMethod(methodName: string, result: any) { }
}

class MethodVerboseDebugLogger implements IDebugLogger {
  logFactory(information: any) { }

  logDecoration(information: any) { }

  logBeforeMethod(methodName: string, args: any) {
    console.log(`DEBUG: calling "${methodName}" with arguments`, args);
  }

  logAfterMethod(methodName: string, result: any) {
    console.log(`DEBUG: called "${methodName}" with result`, result);
  }
}

const DEBUG_LOGGERS = {
  [DebugVerbosity.methodInvocation]: new MethodInvocationDebugLogger(),
  [DebugVerbosity.methodVerbose]: new MethodVerboseDebugLogger()
}

export function Debug(options?: { verbosity?: DebugVerbosity }) {
  let logger: IDebugLogger;

  if (options && options.verbosity)
    logger = DEBUG_LOGGERS[options.verbosity];
  else
    logger = DEBUG_LOGGERS[DebugVerbosity.methodVerbose];

  logger.logFactory(options);

  return function (target: any, propertyKey: string, descriptor: PropertyDescriptor) {

    logger.logDecoration(arguments);

    let originalFn = descriptor.value;
    descriptor.value = function () {

      logger.logBeforeMethod(propertyKey, arguments);

      let result = originalFn.apply(this, arguments);

      logger.logAfterMethod(propertyKey, result);

      return result;
    }
  };
}