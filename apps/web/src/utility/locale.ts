// Resources
import { createServerFn } from "@tanstack/react-start";
import { readdir } from "node:fs/promises";

/** Returns a list of all the locales that exist in the system. */
export const getLocales = createServerFn({ method: "GET" }).handler(async () => {
  const directory = await readdir(`${process.cwd()}/src/locales`);
  return directory.map((file) => file.split(".")[0]).map((locale) => locale.toUpperCase());
});
