// Resources
import { getUser } from "@/lib/user";
import { createFileRoute, redirect } from "@tanstack/react-router";

/** A layout for protecting pages to authenticated user's. */
export const Route = createFileRoute("/_authenticated")({
  beforeLoad: async () => {
    const user = await getUser();

    /* If user session does not exist, redirect to login page. */
    if (!user) {
      throw redirect({ to: "/auth/login" });
    }

    return { user };
  },
});
