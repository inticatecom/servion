// Resources
import { createEnv } from "@t3-oss/env-core";
import { z } from "zod";

/** The repository's base configuration. */
const configuration = {
  logScopes: ["my-app", "web"],
  webPort: 3000
} as const;

/** Public environment variables available to the frontend. */
export const clientEnvironment = createEnv({
  client: {
    PUBLIC_SERVER_URL: z.string()
  },

  clientPrefix: "PUBLIC_",

  emptyStringAsUndefined: true,

  runtimeEnv: import.meta.env ?? (typeof process === "undefined" ? {} : process.env)
});

/** The repository's server environment variables. */
export const environment = createEnv({
  emptyStringAsUndefined: true,

  runtimeEnv: process.env,
  server: {
    DATABASE_URL: z.string(),
    DOCKER_PATH: z.string(),
    FORCE_COLOR: z.coerce.number().optional().default(1),
    NODE_ENV: z.enum(["production", "development"]).optional().default("development"),
    SERVER_PORT: z.coerce.number(),
    SMTP_FROM_ADDRESS: z.email(),
    SMTP_FROM_NAME: z.string(),
    SMTP_HOST: z.string(),
    SMTP_PASSWORD: z.string(),
    SMTP_PORT: z.string(),
    SMTP_USER: z.string()
  }
});

export default configuration;
