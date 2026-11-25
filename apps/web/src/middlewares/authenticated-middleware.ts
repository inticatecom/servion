// Resources
import { createMiddleware } from "@tanstack/react-start";
import { isUser, type User } from "@/lib/auth";
import { environment } from "@servion/config";
import type { Possible } from "@servion/utility";

export interface AuthenticationContext {
  user: Possible<User>;
}

/** Forward the incoming cookie to the API to verify authentication. */
const authenticatedMiddleware = createMiddleware().server(async ({ next, request }) => {
  const response = await fetch(`http://localhost:${environment.SERVER_PORT}/api/users`, {
    cache: "no-store",
    headers: {
      Cookie: request.headers.get("cookie") ?? "",
    },
    method: "GET",
  });

  if (response.status === 401) {
    return next<AuthenticationContext>({ context: { user: undefined } });
  }

  if (!response.ok) {
    throw new Error("Unable to verify authentication");
  }

  const data: unknown = await response.json();

  if (!isUser(data)) {
    throw new Error("Invalid user response");
  }

  return next<AuthenticationContext>({
    context: {
      user: data,
    },
  });
});

export default authenticatedMiddleware;
