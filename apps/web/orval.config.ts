// Resources
import { defineConfig } from "orval";
import { environment } from "@servion/config";

const configuration = defineConfig({
  api: {
    input: {
      target: `http://localhost:${environment.SERVER_PORT}/openapi/v1.json`,
    },
    output: {
      clean: true,
      client: "react-query",
      httpClient: "fetch",
      mode: "tags-split",
      override: {
        query: {
          options: {
            staleTime: 10_000,
          },
          signal: true,
          usePrefetch: true,
        },
      },
      schemas: "./src/generated/api/models",
      target: "./src/generated/api/endpoints.ts",
    },
  },
});

export default configuration;
