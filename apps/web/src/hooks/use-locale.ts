// Resources
import Cookie from "js-cookie";

// Hooks
import { useCallback, useEffect, useState } from "react";

// Constants
export const LOCALE_COOKIE_KEY = "locale";
export const DEFAULT_LOCALE = "EN";

/** A hook for managing the current locale in the user's browser. */
// oxlint-disable-next-line typescript/explicit-function-return-type typescript/explicit-module-boundary-types jsdoc/require-returns
const useLocale = () => {
  // States
  const [locale, setLocale] = useState<string>();

  /** Store cookie in browser during initialization. */
  useEffect(() => {
    if (!Cookie.get(LOCALE_COOKIE_KEY)) {
      Cookie.set(LOCALE_COOKIE_KEY, DEFAULT_LOCALE, { expires: 365 });
    }

    setLocale(Cookie.get(LOCALE_COOKIE_KEY));
  }, []);

  /**
   * Switches to a new locale.
   *
   * @param newLocale The new locale to set.
   */
  const switchLocale = useCallback((newLocale: string) => {
    Cookie.set(LOCALE_COOKIE_KEY, newLocale);
    setLocale(newLocale);
  }, []);

  return { locale, switchLocale };
};

export default useLocale;
