// Resources
import { paraglideMiddleware } from "./paraglide/server";
import handler from "@tanstack/react-start/server-entry";

/** Ensure that server loads Paraglide translations before we serve the pages to the client. */
export default {
  fetch: async (req: Request): Promise<Response> =>
    paraglideMiddleware(req, async () => handler.fetch(req)),
};
