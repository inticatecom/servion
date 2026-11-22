// Resources
import { createFileRoute } from "@tanstack/react-router";
import type { JSX } from "react";

// Components
import { Button } from "@/components/base/buttons/button.tsx";

/** Placeholder for the public login page. */
const Login = (): JSX.Element => <Button color={"primary"}>{"Hello world!"}</Button>;

/** Public login destination for unauthenticated visitors. */
export const Route = createFileRoute("/auth/login")({
  component: Login,
});
