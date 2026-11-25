// Resources
import { REGISTER_USER } from "@servion/validation";
import { createFileRoute } from "@tanstack/react-router";
import type { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";

// Hooks
import { useForm } from "react-hook-form";
import { HookForm } from "@/components/base/form/hook-form";
import CardForm from "@/components/ui/card-form";

export type RegistrationValues = z.infer<typeof REGISTER_USER>;

/** The register page. */
const Register = () => {
  // Hooks
  const form = useForm<RegistrationValues>({
    defaultValues: {
      email: "",
      name: "",
      password: "",
    },
    resolver: zodResolver(REGISTER_USER),
  });

  return (
    <div className={"w-full h-screen flex justify-center items-center"}>
      <HookForm form={form}>
        <CardForm
          title={"Register"}
          description={"Register for a new account into the system."}
          action={{
            text: "Register",
          }}>
          <p>{"Hello world!"}</p>
        </CardForm>
      </HookForm>
    </div>
  );
};

/** The route to serve the component. */
export const Route = createFileRoute("/auth/register")({
  component: Register,
});
