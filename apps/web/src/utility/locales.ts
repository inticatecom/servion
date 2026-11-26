// Resources
import { createServerFn } from "@tanstack/react-start";
import { getRequest } from "@tanstack/react-start/server";
import { extractLocaleFromRequest } from "@/paraglide/runtime";

/** Resolves the locale from the request cookie and browser language preferences. */
export const getRequestLocale = createServerFn({ method: "GET" }).handler(() =>
  extractLocaleFromRequest(getRequest()),
);
