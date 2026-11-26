// Resources
import { createFileRoute } from "@tanstack/react-router";
import { zodResolver } from "@hookform/resolvers/zod";
import { RESET_PASSWORD } from "@servion/validation";
import type { z } from "zod";
import { m } from "@/paraglide/messages";

// Hooks
import { Controller, useForm } from "react-hook-form";
import { useCallback } from "react";

// Components
import { HookForm } from "@/components/base/form/hook-form";
import CardForm from "@/components/ui/card-form";
import { Input } from "@/components/base/input/input";
import { Button } from "@/components/base/buttons/button";

/** The values required for resetting a user's password. */
export type ResetPasswordValues = z.infer<typeof RESET_PASSWORD>;

/** The reset password page. */
const ResetPassword = () => {
  // Hooks
  const form = useForm<ResetPasswordValues>({
    defaultValues: {
      email: "",
    },
    resolver: zodResolver(RESET_PASSWORD),
  });

  /** Handles the process of resetting a user's password. */
  const handleResetPassword = useCallback((data: ResetPasswordValues) => {
    console.log(data);
  }, []);

  return (
    <div className={"w-full h-screen flex justify-center items-center"}>
      <HookForm
        form={form}
        onSubmit={(event) => {
          void form.handleSubmit(handleResetPassword)(event);
        }}
      >
        <CardForm
          title={m["headings.reset_password"]()}
          description={m["pages.reset_password.description"]()}
          action={{
            accent: (
              <Button href={"/auth/login"} color={"secondary"}>
                {m["common.cancel"]()}
              </Button>
            ),
            text: m["common.reset"](),
          }}
        >
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
                hint={fieldState.error?.message ?? m["pages.login.field_descriptions.email"]()}
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
export const Route = createFileRoute("/auth/reset-password")({
  component: ResetPassword,
});
