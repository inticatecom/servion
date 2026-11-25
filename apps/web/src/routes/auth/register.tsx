// Resources
import { REGISTER_USER } from "@servion/validation";
import { createFileRoute } from "@tanstack/react-router";
import type { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";

// Hooks
import { useForm, Controller } from "react-hook-form";

// Components
import { HookForm } from "@/components/base/form/hook-form";
import CardForm from "@/components/ui/card-form";
import { Input } from "@/components/base/input/input";

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
          }}
          notice={{
            text: "Already have an Account",
            url: "/auth/login",
          }}>
          <div className={"flex gap-2"}>
            <Controller
              name={"name"}
              control={form.control}
              render={({ field, fieldState }) => (
                <Input
                  label={"Full Name"}
                  placeholder={"John Doe"}
                  name={field.name}
                  value={field.value}
                  onChange={field.onChange}
                  onBlur={field.onBlur}
                  ref={field.ref}
                  hint={fieldState.error?.message ?? "Your full name."}
                  isInvalid={fieldState.error !== undefined}
                />
              )}
            />
            <Controller
              name={"email"}
              control={form.control}
              render={({ field, fieldState }) => (
                <Input
                  label={"Email"}
                  placeholder={"john@doe.com"}
                  name={field.name}
                  value={field.value}
                  onChange={field.onChange}
                  onBlur={field.onBlur}
                  ref={field.ref}
                  hint={
                    fieldState.error?.message ??
                    "The email address of your account."
                  }
                  isInvalid={fieldState.error !== undefined}
                />
              )}
            />
          </div>
          <Controller
            name={"password"}
            control={form.control}
            render={({ field, fieldState }) => (
              <Input
                label={"Password"}
                placeholder={"********"}
                name={field.name}
                value={field.value}
                onChange={field.onChange}
                onBlur={field.onBlur}
                ref={field.ref}
                hint={
                  fieldState.error?.message ??
                  "The password you want to set for the account."
                }
                isInvalid={fieldState.error !== undefined}
                type={"password"}
              />
            )}
          />
        </CardForm>
      </HookForm>
    </div>
  );
};

/** The route to serve the component. */
export const Route = createFileRoute("/auth/register")({
  component: Register,
});
