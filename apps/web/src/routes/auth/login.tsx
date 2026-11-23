// Resources
import { createFileRoute } from "@tanstack/react-router";
import type { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";

// Schemas
import { AUTHENTICATE_USER } from "@servion/validation";

// Hooks
import { useForm, Controller } from "react-hook-form";
import { useCallback, useState } from "react";
import { usePutApiUsers } from "@/generated/api/user/user";

// Components
import { Button } from "@/components/base/buttons/button";
import { HookForm } from "@/components/base/form/hook-form";
import { Input } from "@/components/base/input/input";
import Callout from "@/components/ui/callout";

/** The output of the validator for authenticated the user. */
type AuthenticationValues = z.infer<typeof AUTHENTICATE_USER>;

/** Placeholder for the public login page. */
const Login = () => {
  // States
  const [errored, setErrored] = useState<boolean>();

  // Hooks
  const form = useForm<AuthenticationValues>({
    defaultValues: {
      email: "",
      password: "",
    },
    resolver: zodResolver(AUTHENTICATE_USER),
  });

  /** The query members for authenticated users. */
  const { mutateAsync: authenticate, isPending: authenticating } =
    usePutApiUsers();

  /** Handles the authentication for the form. */
  const handleAuthentication = useCallback(
    async (data: AuthenticationValues) => {
      setErrored(false);

      const response = await authenticate({
        data,
      });

      if (response.status !== 200) {
        setErrored(true);
      }
    },
    [authenticate],
  );

  return (
    <div className={"w-full h-screen flex justify-center items-center"}>
      <HookForm
        form={form}
        className={"bg-primary border border-white/10 rounded-lg p-10 prose"}
        onSubmit={(event) => {
          void form.handleSubmit(handleAuthentication)(event);
        }}>
        <h1>{"Login"}</h1>
        {errored && (
          <Callout type={"error"}>
            {"The provided credentials were invalid."}
          </Callout>
        )}
        <p>
          {
            "Authenticate into your existing account using the credentials you set."
          }
        </p>
        <section className={"mb-3 flex flex-col gap-2"}>
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

          <Controller
            name={"password"}
            control={form.control}
            render={({ field, fieldState }) => (
              <Input
                label={"Password"}
                placeholder={"****"}
                name={field.name}
                value={field.value}
                onChange={field.onChange}
                onBlur={field.onBlur}
                ref={field.ref}
                type={"password"}
                hint={
                  fieldState.error?.message ?? "The password of your account."
                }
                isInvalid={fieldState.error !== undefined}
              />
            )}
          />
        </section>
        <div className={"flex w-full justify-between"}>
          <Button href={"#"} color={"link-color"}>
            {"Forgot Password"}
          </Button>
          <div className={"flex gap-2"}>
            <Button
              type={"button"}
              color={"secondary"}
              isDisabled={authenticating}>
              {"Register"}
            </Button>
            <Button type={"submit"} isLoading={authenticating}>
              {"Login"}
            </Button>
          </div>
        </div>
      </HookForm>
    </div>
  );
};

/** Public login destination for unauthenticated visitors. */
export const Route = createFileRoute("/auth/login")({
  component: Login,
});
