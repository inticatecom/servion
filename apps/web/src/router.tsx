// Resources
import { createRouter } from "@tanstack/react-router";
import { setupRouterSsrQueryIntegration } from "@tanstack/react-router-ssr-query";
import { createQueryClient } from "@/lib/query-client";
import { routeTree } from "./routeTree.gen";

// Components
import DefaultCatchBoundary from "./components/default-catch-boundary";
import NotFound from "./components/not-found";

// oxlint-disable-next-line typescript/explicit-module-boundary-types typescript/explicit-function-return-type
export const getRouter = () => {
  const queryClient = createQueryClient();
  const router = createRouter({
    context: { queryClient },
    defaultErrorComponent: DefaultCatchBoundary,
    defaultNotFoundComponent: () => <NotFound />,
    defaultPreload: "intent",
    defaultPreloadStaleTime: 0,
    routeTree,
    scrollRestoration: true
  });

  /* Provide the query client and hydrate its cache across server rendering. */
  setupRouterSsrQueryIntegration({ queryClient, router });

  return router;
};
