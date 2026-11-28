/// <reference types="vite/client" />
// Resources
import { HeadContent, Scripts, createRootRouteWithContext } from "@tanstack/react-router";
import type { RouterContext } from "@/lib/query-client";
import appCss from "@/styles/app.css?url";
import { seo } from "@/utility/seo";
import type { Children } from "@servion/utility";
import { getRequestLocale } from "@/utility/locales";
import { baseLocale } from "@/paraglide/runtime";

// Components
import DefaultCatchBoundary from "@/components/default-catch-boundary";
import NotFound from "@/components/not-found";
import Footer from "@/components/ui/layout/footer";
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools";

/** The root document to render content inside. */
const RootDocument = ({ children }: Children) => {
  // Hooks
  // oxlint-disable-next-line no-use-before-define
  const locale = Route.useLoaderData()?.locale ?? baseLocale;

  return (
    <html lang={locale} className={"dark dark-mode antialiased"}>
      <head>
        <HeadContent />
      </head>
      <body className={"bg-primary relative min-h-screen"}>
        <main>{children}</main>
        <Footer locale={locale} />
        <TanStackRouterDevtools position={"bottom-right"} />
        <Scripts />
      </body>
    </html>
  );
};

/** The root route of the application. */
export const Route = createRootRouteWithContext<RouterContext>()({
  errorComponent: DefaultCatchBoundary,
  head: () => ({
    links: [
      { href: appCss, rel: "stylesheet" },
      {
        href: "/apple-touch-icon.png",
        rel: "apple-touch-icon",
        sizes: "180x180",
      },
      {
        href: "/favicon-32x32.png",
        rel: "icon",
        sizes: "32x32",
        type: "image/png",
      },
      {
        href: "/favicon-16x16.png",
        rel: "icon",
        sizes: "16x16",
        type: "image/png",
      },
      { color: "#121212", href: "/site.webmanifest", rel: "manifest" },
      { href: "/favicon.ico", rel: "icon" },
    ],
    meta: [
      {
        charSet: "utf8",
      },
      {
        content: "width=device-width, initial-scale=1",
        name: "viewport",
      },
      ...seo({
        description: `Manage your game servers with Servion.`,
        title: "Servion",
      }),
    ],
  }),
  loader: async () => ({
    locale: await getRequestLocale(),
  }),
  notFoundComponent: () => <NotFound />,
  shellComponent: RootDocument,
});
