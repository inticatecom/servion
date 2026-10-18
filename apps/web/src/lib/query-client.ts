// Resources
import { QueryClient } from "@tanstack/react-query";

/** Shared dependencies available to route loaders and components. */
export interface RouterContext {
  queryClient: QueryClient;
}

/**
 * Create an isolated query cache for each router and server request.
 * @returns A query client with the application's default freshness window.
 */
export const createQueryClient = (): QueryClient =>
  new QueryClient({
    defaultOptions: {
      queries: {
        staleTime: 10_000
      }
    }
  });
