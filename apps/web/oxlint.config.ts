// Resources
import baseConfig from "../../oxlint.config.ts";
import { defineConfig } from "oxlint";

const configuration = defineConfig({
  extends: [baseConfig],
  ignorePatterns: [
    "routeTree.gen.ts",
    "**/generated/**",
    "src/components/base/**",
    "src/components/foundations/**",
    "src/utils/**",
    "src/hooks/use-resize-observer.ts",
    "src/paraglide/**",
  ],
  overrides: [
    {
      files: ["**/*.tsx"],
      rules: {
        "typescript/explicit-function-return-type": "off",
        "typescript/explicit-module-boundary-types": "off",
        "typescript/only-throw-error": "off",
      },
    },
  ],
  rules: {
    "eslint/no-console": "off",
    "eslint/no-void": "off",
    "eslint/sort-imports": "off",
    "oxc/no-optional-chaining": "off",
    "typescript/no-floating-promises": "error",
    "typescript/prefer-readonly-parameter-types": "off",
    "typescript/strict-boolean-expressions": "off",
    "unicorn/prefer-global-this": "off",
  },
});

export default configuration;
