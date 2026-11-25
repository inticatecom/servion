// Components
import { Select } from "@/components/base/select/select";

// Hooks
import useLocale from "@/hooks/use-locale";
import { useCallback } from "react";

// Constants
const YEAR = new Date().getFullYear();

/** The properties of the footer component. */
export interface FooterProperties {
  locales: string[];
}

/** The application's base footer. */
const Footer = ({ locales }: FooterProperties) => {
  // Hooks
  const { locale: selectedLocale, switchLocale } = useLocale();

  const handleLocaleChange = useCallback(
    (locale: string) => {
      switchLocale(locale);
    },
    [switchLocale],
  );

  return (
    <footer
      className={
        "grid grid-cols-3 items-center p-3 w-full absolute bottom-0 mx-auto"
      }>
      <Select
        className={"justify-self-start"}
        value={selectedLocale}
        placeholder={"Language"}
        onChange={(value) => {
          handleLocaleChange(String(value));
        }}>
        {locales.map((locale) => (
          <Select.Item key={locale} id={locale}>
            {locale}
          </Select.Item>
        ))}
      </Select>
      <section className={"prose justify-self-center"}>
        <p>{`© ${YEAR} - ${YEAR + 1} Servion`}</p>
      </section>
    </footer>
  );
};

export default Footer;
