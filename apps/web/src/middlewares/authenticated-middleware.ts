// Resources
import { createMiddleware } from "@tanstack/react-start";
import { redirect } from "@tanstack/react-router";
import { isUser } from "@/lib/auth.ts";
import { environment } from "@servion/config";

/**
 * Forward the incoming cookie to the API to verify authentication.
 * Redirect unauthorized requests to log in and pass a recognized user to downstream context.
 */
const authenticatedMiddleware = createMiddleware().server(async ({ next, request }) => {
  const response = await fetch(`http://localhost:${environment.SERVER_PORT}/api/users`, {
    cache: "no-store",
    headers: {
      Cookie: request.headers.get("cookie") ?? "",
    },
    method: "GET",
  });

  if (response.status === 401) {
    return redirect({ to: "/auth/login" });
  }

  if (!response.ok) {
    throw new Error("Unable to verify authentication");
  }

  const data: unknown = await response.json();

  if (!isUser(data)) {
    throw new Error("Invalid user response");
  }

  return next({
    context: {
      user: data,
    },
  });
});

export default authenticatedMiddleware;
