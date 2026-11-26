// Resources
import { isLocale, locales, setLocale, type Locale } from "@/paraglide/runtime";

// Components
import { Select } from "@/components/untitled/base/select/select";

// Constants
const YEAR = new Date().getFullYear();

/** The properties of the footer component. */
export interface FooterProperties {
  locale: Locale;
}

/** The application's base footer. */
const Footer = ({ locale }: FooterProperties) => (
  <footer className={"grid grid-cols-3 items-center p-3 w-full absolute bottom-0 mx-auto"}>
    <Select
      className={"justify-self-start"}
      aria-label={"Language"}
      value={locale}
      placeholder={"Language"}
      onChange={(value) => {
        if (isLocale(value) && value !== locale) {
          void Promise.resolve(setLocale(value, { reload: false })).then(() => {
            window.location.reload();
          });
        }
      }}
    >
      {locales.map((option) => (
        <Select.Item key={option} id={option}>
          {option.toUpperCase()}
        </Select.Item>
      ))}
    </Select>
    <section className={"prose justify-self-center"}>
      <p>{`© ${YEAR} - ${YEAR + 1} Servion`}</p>
    </section>
  </footer>
);

export default Footer;
