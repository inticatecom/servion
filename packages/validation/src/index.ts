// Resources
import { z } from "zod";

/** The schema for creating a user. */
export const AUTHENTICATE_USER = z.object({
  email: z.email(),
  password: z.string()
});
