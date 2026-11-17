// Resources
import { createLogger } from "@servion/utility";

/* Re-export the client-safe class name utility. */
export { cn } from "@/lib/utils";

/** The base logger for the website application. */
export const logger = createLogger("web");
