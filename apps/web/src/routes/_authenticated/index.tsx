// Resources
import { createFileRoute } from "@tanstack/react-router";
import { Route as AuthRoute } from "../_authenticated";

/** The home page of the panel. */
const Home = () => {
  // Hooks
  const { user } = AuthRoute.useRouteContext();

  return <p>{`Email: ${user.email}`}</p>;
};

/** The home page route. */
export const Route = createFileRoute("/_authenticated/")({
  component: Home,
});
