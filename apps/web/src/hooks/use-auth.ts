// Resources
import { useQuery } from "@tanstack/react-query";
import { getCurrentUser } from "@/lib/auth.ts";

// oxlint-disable-next-line typescript/explicit-function-return-type typescript/explicit-module-boundary-types
const useAuth = () => {
  const query = useQuery({
    queryFn: getCurrentUser,
    queryKey: ["auth", "me"],
    retry: false,
    staleTime: 60_000,
  });

  return {
    isAuthenticated: query.isSuccess && query.data !== null,
    isError: query.isError,
    isLoading: query.isPending,
    user: query.data,
  };
};

export default useAuth;
