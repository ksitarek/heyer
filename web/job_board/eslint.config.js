import eslint from "@eslint/js";
import { configs as ngConfigs, processInlineTemplates } from "angular-eslint";
import prettierConfig from "eslint-config-prettier";
import jasminePlugin from "eslint-plugin-jasmine";
import { configs as jsoncConfigs } from "eslint-plugin-jsonc";
import globals from "globals";
import { config, configs as tsConfigs } from "typescript-eslint";

export default config(
  { ignores: [".angular/*", "dist/*", "libs/ui/*"] },
  {
    files: ["**/*.js"],
    extends: [eslint.configs.recommended, prettierConfig],
    languageOptions: {
      globals: {
        ...globals.node,
      },
    },
    rules: {},
  },
  {
    files: ["**/*.ts"],
    extends: [
      eslint.configs.recommended,
      ...tsConfigs.strictTypeChecked,
      ...tsConfigs.stylisticTypeChecked,
      ...ngConfigs.tsAll,
      prettierConfig,
    ],
    languageOptions: {
      parserOptions: {
        projectService: true,
        tsconfigRootDir: import.meta.dirname,
      },
    },
    processor: processInlineTemplates,
    rules: {
      "@angular-eslint/directive-selector": [
        "error",
        {
          type: "attribute",
          prefix: "app",
          style: "camelCase",
        },
      ],
      "@angular-eslint/prefer-on-push-component-change-detection": "off",
      "@typescript-eslint/no-extraneous-class": "off",
      "@typescript-eslint/no-empty-function": "off",
      "@typescript-eslint/unbound-method": [
        "error",
        {
          ignoreStatic: true,
        },
      ],
      "@angular-eslint/component-selector": [
        "error",
        {
          type: "element",
          prefix: "h",
          style: "kebab-case",
        },
      ],
    },
  },
  {
    files: ["**/*.html"],
    extends: [...ngConfigs.templateAll, ...ngConfigs.templateAccessibility],
    rules: {
      "@angular-eslint/template/i18n": "off",
      "@angular-eslint/template/no-call-expression": "off",
      "@angular-eslint/template/label-has-associated-control": "off",
      "@angular-eslint/template/prefer-ngsrc": "off",
    },
  },
  {
    files: ["**/*.json"],
    extends: [...jsoncConfigs["flat/recommended-with-jsonc"], ...jsoncConfigs["flat/prettier"]],
    rules: {},
  },
  {
    files: ["src/**/*.spec.ts"],
    extends: [jasminePlugin.configs.recommended, prettierConfig],
    languageOptions: {
      globals: {
        ...globals.jasmine,
      },
    },
    plugins: { jasmine: jasminePlugin },
    rules: {},
  },
);
