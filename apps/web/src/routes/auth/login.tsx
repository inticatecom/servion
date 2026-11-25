// Resources
import { createFileRoute, redirect } from "@tanstack/react-router";
import type { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { getUser } from "@/lib/user";

// Schemas
import { AUTHENTICATE_USER } from "@servion/validation";

// Hooks
import { useForm, Controller } from "react-hook-form";
import { useCallback, useEffect, useState } from "react";
import { usePutApiUsers } from "@/generated/api/user/user";
import useLocale from "@/hooks/use-locale";

// Components
import { Button } from "@/components/base/buttons/button";
import { HookForm } from "@/components/base/form/hook-form";
import { Input } from "@/components/base/input/input";
import CardForm from "@/components/ui/card-form";

/** The output of the validator for authenticated the user. */
type AuthenticationValues = z.infer<typeof AUTHENTICATE_USER>;

/** The login page allowing users to access their account. */
const Login = () => {
  // States
  const [errored, setErrored] = useState<boolean>();

  // Hooks
  const { locale } = useLocale();
  const form = useForm<AuthenticationValues>({
    defaultValues: {
      email: "",
      password: "",
    },
    resolver: zodResolver(AUTHENTICATE_USER),
  });

  useEffect(() => {
    console.log(locale);
  }, [locale]);

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
        onSubmit={(event) => {
          void form.handleSubmit(handleAuthentication)(event);
        }}>
        <CardForm
          title={"Login"}
          description={
            "Authenticate into your existing account using the credentials you set."
          }
          action={{
            accent: (
              <Button href={"/auth/register"} color={"secondary"}>
                {"Register"}
              </Button>
            ),
            text: "Login",
          }}
          pending={authenticating}
          error={
            errored ? "The provided credentials were invalid." : undefined
          }>
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
                placeholder={"********"}
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
        </CardForm>
      </HookForm>
    </div>
  );
};

/** The route to serve the component. */
export const Route = createFileRoute("/auth/login")({
  beforeLoad: async () => {
    const user = await getUser();
    if (user) {
      throw redirect({
        to: "/",
      });
    }
  },
  component: Login,
});
