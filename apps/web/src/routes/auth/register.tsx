// Resources
import { REGISTER_USER } from "@servion/validation";
import { createFileRoute } from "@tanstack/react-router";
import type { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { m } from "@/paraglide/messages";
import type { APIError } from "@servion/utility";

// Hooks
import { useForm, Controller } from "react-hook-form";
import { useCallback, useEffect } from "react";
import { usePostApiUsers } from "@/generated/api";

// Components
import { HookForm } from "@/components/untitled/base/form/hook-form";
import CardForm from "@/components/ui/card-form";
import { Input } from "@/components/untitled/base/input/input";

/** The values required to register a new account. */
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

  /** The query members for registering users. */
  const {
    mutateAsync: register,
    isPending: registering,
    error,
  } = usePostApiUsers<APIError>();

  /** Handles the registration process. */
  const handleRegistration = useCallback(
    async (data: RegistrationValues) => {
      await register({
        data: {
          ...data,
          username: data.name,
        },
      });
    },
    [register],
  );

  useEffect(() => {
    console.log(error);
  }, [error]);

  return (
    <div className={"w-full h-screen flex justify-center items-center"}>
      <HookForm
        form={form}
        onSubmit={(event) => {
          void form.handleSubmit(handleRegistration)(event);
        }}>
        <CardForm
          title={m["headings.register"]()}
          description={m["pages.register.description"]()}
          action={{
            text: m["headings.register"](),
          }}
          notice={{
            text: m["pages.register.already_have_account"](),
            url: "/auth/login",
          }}
          error={error?.info?.detail}
          pending={registering}>
          <div className={"flex gap-2"}>
            <Controller
              name={"name"}
              control={form.control}
              render={({ field, fieldState }) => (
                <Input
                  label={m["common.full_name"]()}
                  placeholder={"John Doe"}
                  name={field.name}
                  value={field.value}
                  onChange={field.onChange}
                  onBlur={field.onBlur}
                  ref={field.ref}
                  hint={
                    fieldState.error?.message ??
                    m["pages.register.field_descriptions.name"]()
                  }
                  isInvalid={fieldState.error !== undefined}
                />
              )}
            />
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
                    m["pages.register.field_descriptions.email"]()
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
                label={m["common.password"]()}
                placeholder={"********"}
                name={field.name}
                value={field.value}
                onChange={field.onChange}
                onBlur={field.onBlur}
                ref={field.ref}
                hint={
                  fieldState.error?.message ??
                  m["pages.register.field_descriptions.password"]()
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
