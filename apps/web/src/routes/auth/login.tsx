// Resources
import { createFileRoute } from "@tanstack/react-router";
import type { JSX } from "react";

const Login = (): JSX.Element => <div>{"!!!"}</div>;

export const Route = createFileRoute("/auth/login")({
  component: Login,
});
