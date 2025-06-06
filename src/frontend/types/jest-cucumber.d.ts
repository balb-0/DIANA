declare module "jest-cucumber" {
  export type DefineStepFunction = (
    regex: RegExp | string,
    callback: (...args: any[]) => void | Promise<void>
  ) => void;

  export interface StepDefinitions {
    given: DefineStepFunction;
    when: DefineStepFunction;
    then: DefineStepFunction;
    and: DefineStepFunction;
    but: DefineStepFunction;
  }

  export function loadFeature(featurePath: string): any;
  export function defineFeature(
    feature: any,
    test: (
      test: (title: string, callback: (steps: StepDefinitions) => void) => void
    ) => void
  ): void;
}
