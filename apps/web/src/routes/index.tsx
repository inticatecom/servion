// Resources
import { createFileRoute } from "@tanstack/react-router";
import { requireUser } from "@/lib/user";

/** The server ownership dashboard mounted at the root. */
const Home = () => <p>{"Hello world!"}</p>;

/** Authenticate visitors before loading the dashboard and expose their user in route context. */
export const Route = createFileRoute("/")({
  beforeLoad: async () => ({ user: await requireUser() }),
  component: Home,
});
