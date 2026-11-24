// Resources
import { createFileRoute } from "@tanstack/react-router";

// Hooks
import { useForm } from "react-hook-form";

// Components
import { HookForm } from "@/components/base/form/hook-form";
import Card from "@/components/ui/card";

/** The reset password page. */
const ResetPassword = () => {
  // Hooks
  const form = useForm();

  return (
    <div className={"w-full h-screen flex justify-center items-center"}>
      <HookForm form={form}>
        <Card className={"prose"}>
          <h1>{"Reset Password"}</h1>
        </Card>
      </HookForm>
    </div>
  );
};

/** The route to serve the component.. */
export const Route = createFileRoute("/auth/reset-password")({
  component: ResetPassword,
});
