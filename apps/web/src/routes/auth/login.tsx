// Resources
import { createFileRoute } from "@tanstack/react-router";
import type { JSX } from "react";

/** Placeholder for the public login page. */
const Login = (): JSX.Element => <div>{"!!!"}</div>;

/** Public login destination for unauthenticated visitors. */
export const Route = createFileRoute("/auth/login")({
  component: Login,
});
