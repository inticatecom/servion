// Resources
import { useQuery } from "@tanstack/react-query";
import { getCurrentUser } from "@/lib/auth.ts";

/**
 * Query the current user with a one-minute freshness window and expose the request state.
 * @returns The user and authentication, loading, and error indicators.
 */
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
