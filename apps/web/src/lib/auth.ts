// Resources
import type { Possible } from "@servion/utility";
import { clientEnvironment } from "@servion/config";

/** The structure of an authenticated user. */
export interface User {
  id: string;
  userName: string;
  email: string;
}

export const isUser = (data: unknown): data is User =>
  typeof data === "object" && data !== null && "id" in data;

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
