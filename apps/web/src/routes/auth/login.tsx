// Resources
import { createFileRoute, redirect } from "@tanstack/react-router";
import type { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { getUser } from "@/lib/user";
import { m } from "@/paraglide/messages";
import type { APIError } from "@servion/utility";

// Schemas
import { AUTHENTICATE_USER } from "@servion/validation";

// Hooks
import { useForm, Controller } from "react-hook-form";
import { useCallback } from "react";
import { usePutApiUsers } from "@/generated/api/user/user";

// Components
import { Button } from "@/components/untitled/base/buttons/button";
import { HookForm } from "@/components/untitled/base/form/hook-form";
import { Input } from "@/components/untitled/base/input/input";
import CardForm from "@/components/ui/card-form";

/** The output of the validator for authenticated the user. */
type AuthenticationValues = z.infer<typeof AUTHENTICATE_USER>;

/** The login page allowing users to access their account. */
const Login = () => {
  // Hooks
  const form = useForm<AuthenticationValues>({
    defaultValues: {
      email: "",
      password: "",
    },
    resolver: zodResolver(AUTHENTICATE_USER),
  });

  /** The query members for authenticating users. */
  const {
    mutateAsync: authenticate,
    isPending: authenticating,
    error,
  } = usePutApiUsers<APIError>();

  /** Handles the authentication for the form. */
  const handleAuthentication = useCallback(
    async (data: AuthenticationValues) => {
      await authenticate({
        data,
      });
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
          title={m["headings.login"]()}
          description={m["pages.login.description"]()}
          action={{
            accent: (
              <Button href={"/auth/register"} color={"secondary"}>
                {m["headings.register"]()}
              </Button>
            ),
            text: m["headings.login"](),
          }}
          pending={authenticating}
          error={error?.info?.detail}
          notice={{
            text: m["pages.login.forgot_password"](),
            url: "/auth/reset-password",
          }}>
          <Controller
            name={"email"}
            control={form.control}
            render={({ field, fieldState }) => (
              <Input
                label={m["common.email"]()}
                placeholder={"john@doe.com"}
                name={field.name}
                value={field.value}
                onChange={field.onChange}
                onBlur={field.onBlur}
                ref={field.ref}
                hint={
                  fieldState.error?.message ??
                  m["pages.login.field_descriptions.email"]()
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
                label={m["common.password"]()}
                placeholder={"********"}
                name={field.name}
                value={field.value}
                onChange={field.onChange}
                onBlur={field.onBlur}
                ref={field.ref}
                type={"password"}
                hint={
                  fieldState.error?.message ??
                  m["pages.login.field_descriptions.password"]()
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
