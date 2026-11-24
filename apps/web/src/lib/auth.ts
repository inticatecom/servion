// Resources
import type { Possible } from "@servion/utility";
import { clientEnvironment } from "@servion/config";

/** The structure of an authenticated user. */
export interface User {
  id: string;
  userName: string;
  email: string;
}

/**
 * Check that a response is an object containing a user ID; other fields are not validated.
 *
 * @param data - The response value to inspect.
 *
 * @returns Whether the value is a non-null object containing an ID property.
 */
export const isUser = (data: unknown): data is User =>
  typeof data === "object" && data !== null && "id" in data;

/**
 * Fetch the current user using the browser's authentication cookie.
 *
 * @throws {Error} HTTP failures other than unauthorized responses.
 *
 * @returns The current user, or undefined for unauthorized or unrecognized responses.
 */
export const getCurrentUser = async (): Promise<Possible<User>> => {
  const response = await fetch(`${clientEnvironment.PUBLIC_SERVER_URL}/api/users`, {
    credentials: "include",
    method: "GET",
  });

  if (response.status === 401) {
    return undefined;
  }

  if (!response.ok) {
    throw new Error("Failed to fetch user information.");
  }

  const result = (await response.json()) as unknown;
  if (!isUser(result)) {
    return undefined;
  }

  return result;
};
