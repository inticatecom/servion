// Resources
import baseConfig from "../../oxfmt.config.ts";
import { defineConfig } from "oxfmt";

const configuration = defineConfig({
  extends: [baseConfig],
  ignorePatterns: [
    "**/public/**",
    "**/build/**",
    "**/routeTree.gen.ts",
    "**/generated/**",
    "src/components/base/**",
    "src/utils/**",
  ],
});

export default configuration;
