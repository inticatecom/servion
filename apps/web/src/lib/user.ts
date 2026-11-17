// Resources
import { createServerFn } from "@tanstack/react-start";
import authenticatedMiddleware from "@/middlewares/authenticated-middleware.ts";

/** Resolve the authenticated user on the server before a protected route loads. */
export const requireUser = createServerFn({ method: "GET" })
  .middleware([authenticatedMiddleware])
  .handler(({ context }) => context.user);
