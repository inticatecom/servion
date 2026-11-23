// Resources
import { z } from "zod";

/** The schema for authenticating a user. */
export const AUTHENTICATE_USER = z.object({
  email: z.email("The provided email address is invalid."),
  password: z.string().min(3, "Password must be at least 3 characters long.")
});
