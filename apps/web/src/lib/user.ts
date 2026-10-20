import { createServerFn } from "@tanstack/react-start";
import authenticatedMiddleware from "@/middlewares/authenticated-middleware.ts";

export const requireUser = createServerFn({ method: "GET" })
  .middleware([authenticatedMiddleware])
  .handler(({ context }) => context.user);
