// Resources
import { getUser } from "@/lib/user";
import { createFileRoute, redirect } from "@tanstack/react-router";

/** The server ownership dashboard mounted at the root. */
const Home = () => <p>{"Hello world!"}</p>;

/** Authenticate visitors before loading the dashboard and expose their user in route context. */
export const Route = createFileRoute("/")({
  beforeLoad: async () => {
    const user = await getUser();
    if (!user) {
      throw redirect({
        to: "/auth/login",
      });
    }
  },
  component: Home,
});
