\connect keycloak
--
-- PostgreSQL database dump
--

-- Dumped from database version 12.1 (Debian 12.1-1.pgdg100+1)
-- Dumped by pg_dump version 12.1 (Debian 12.1-1.pgdg100+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: admin_event_entity; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.admin_event_entity (
    id character varying(36) NOT NULL,
    admin_event_time bigint,
    realm_id character varying(255),
    operation_type character varying(255),
    auth_realm_id character varying(255),
    auth_client_id character varying(255),
    auth_user_id character varying(255),
    ip_address character varying(255),
    resource_path character varying(2550),
    representation text,
    error character varying(255),
    resource_type character varying(64)
);


ALTER TABLE public.admin_event_entity OWNER TO keycloak;

--
-- Name: associated_policy; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.associated_policy (
    policy_id character varying(36) NOT NULL,
    associated_policy_id character varying(36) NOT NULL
);


ALTER TABLE public.associated_policy OWNER TO keycloak;

--
-- Name: authentication_execution; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.authentication_execution (
    id character varying(36) NOT NULL,
    alias character varying(255),
    authenticator character varying(36),
    realm_id character varying(36),
    flow_id character varying(36),
    requirement integer,
    priority integer,
    authenticator_flow boolean DEFAULT false NOT NULL,
    auth_flow_id character varying(36),
    auth_config character varying(36)
);


ALTER TABLE public.authentication_execution OWNER TO keycloak;

--
-- Name: authentication_flow; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.authentication_flow (
    id character varying(36) NOT NULL,
    alias character varying(255),
    description character varying(255),
    realm_id character varying(36),
    provider_id character varying(36) DEFAULT 'basic-flow'::character varying NOT NULL,
    top_level boolean DEFAULT false NOT NULL,
    built_in boolean DEFAULT false NOT NULL
);


ALTER TABLE public.authentication_flow OWNER TO keycloak;

--
-- Name: authenticator_config; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.authenticator_config (
    id character varying(36) NOT NULL,
    alias character varying(255),
    realm_id character varying(36)
);


ALTER TABLE public.authenticator_config OWNER TO keycloak;

--
-- Name: authenticator_config_entry; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.authenticator_config_entry (
    authenticator_id character varying(36) NOT NULL,
    value text,
    name character varying(255) NOT NULL
);


ALTER TABLE public.authenticator_config_entry OWNER TO keycloak;

--
-- Name: broker_link; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.broker_link (
    identity_provider character varying(255) NOT NULL,
    storage_provider_id character varying(255),
    realm_id character varying(36) NOT NULL,
    broker_user_id character varying(255),
    broker_username character varying(255),
    token text,
    user_id character varying(255) NOT NULL
);


ALTER TABLE public.broker_link OWNER TO keycloak;

--
-- Name: client; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.client (
    id character varying(36) NOT NULL,
    enabled boolean DEFAULT false NOT NULL,
    full_scope_allowed boolean DEFAULT false NOT NULL,
    client_id character varying(255),
    not_before integer,
    public_client boolean DEFAULT false NOT NULL,
    secret character varying(255),
    base_url character varying(255),
    bearer_only boolean DEFAULT false NOT NULL,
    management_url character varying(255),
    surrogate_auth_required boolean DEFAULT false NOT NULL,
    realm_id character varying(36),
    protocol character varying(255),
    node_rereg_timeout integer DEFAULT 0,
    frontchannel_logout boolean DEFAULT false NOT NULL,
    consent_required boolean DEFAULT false NOT NULL,
    name character varying(255),
    service_accounts_enabled boolean DEFAULT false NOT NULL,
    client_authenticator_type character varying(255),
    root_url character varying(255),
    description character varying(255),
    registration_token character varying(255),
    standard_flow_enabled boolean DEFAULT true NOT NULL,
    implicit_flow_enabled boolean DEFAULT false NOT NULL,
    direct_access_grants_enabled boolean DEFAULT false NOT NULL
);


ALTER TABLE public.client OWNER TO keycloak;

--
-- Name: client_attributes; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.client_attributes (
    client_id character varying(36) NOT NULL,
    value character varying(4000),
    name character varying(255) NOT NULL
);


ALTER TABLE public.client_attributes OWNER TO keycloak;

--
-- Name: client_auth_flow_bindings; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.client_auth_flow_bindings (
    client_id character varying(36) NOT NULL,
    flow_id character varying(36),
    binding_name character varying(255) NOT NULL
);


ALTER TABLE public.client_auth_flow_bindings OWNER TO keycloak;

--
-- Name: client_default_roles; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.client_default_roles (
    client_id character varying(36) NOT NULL,
    role_id character varying(36) NOT NULL
);


ALTER TABLE public.client_default_roles OWNER TO keycloak;

--
-- Name: client_initial_access; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.client_initial_access (
    id character varying(36) NOT NULL,
    realm_id character varying(36) NOT NULL,
    "timestamp" integer,
    expiration integer,
    count integer,
    remaining_count integer
);


ALTER TABLE public.client_initial_access OWNER TO keycloak;

--
-- Name: client_node_registrations; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.client_node_registrations (
    client_id character varying(36) NOT NULL,
    value integer,
    name character varying(255) NOT NULL
);


ALTER TABLE public.client_node_registrations OWNER TO keycloak;

--
-- Name: client_scope; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.client_scope (
    id character varying(36) NOT NULL,
    name character varying(255),
    realm_id character varying(36),
    description character varying(255),
    protocol character varying(255)
);


ALTER TABLE public.client_scope OWNER TO keycloak;

--
-- Name: client_scope_attributes; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.client_scope_attributes (
    scope_id character varying(36) NOT NULL,
    value character varying(2048),
    name character varying(255) NOT NULL
);


ALTER TABLE public.client_scope_attributes OWNER TO keycloak;

--
-- Name: client_scope_client; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.client_scope_client (
    client_id character varying(36) NOT NULL,
    scope_id character varying(36) NOT NULL,
    default_scope boolean DEFAULT false NOT NULL
);


ALTER TABLE public.client_scope_client OWNER TO keycloak;

--
-- Name: client_scope_role_mapping; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.client_scope_role_mapping (
    scope_id character varying(36) NOT NULL,
    role_id character varying(36) NOT NULL
);


ALTER TABLE public.client_scope_role_mapping OWNER TO keycloak;

--
-- Name: client_session; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.client_session (
    id character varying(36) NOT NULL,
    client_id character varying(36),
    redirect_uri character varying(255),
    state character varying(255),
    "timestamp" integer,
    session_id character varying(36),
    auth_method character varying(255),
    realm_id character varying(255),
    auth_user_id character varying(36),
    current_action character varying(36)
);


ALTER TABLE public.client_session OWNER TO keycloak;

--
-- Name: client_session_auth_status; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.client_session_auth_status (
    authenticator character varying(36) NOT NULL,
    status integer,
    client_session character varying(36) NOT NULL
);


ALTER TABLE public.client_session_auth_status OWNER TO keycloak;

--
-- Name: client_session_note; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.client_session_note (
    name character varying(255) NOT NULL,
    value character varying(255),
    client_session character varying(36) NOT NULL
);


ALTER TABLE public.client_session_note OWNER TO keycloak;

--
-- Name: client_session_prot_mapper; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.client_session_prot_mapper (
    protocol_mapper_id character varying(36) NOT NULL,
    client_session character varying(36) NOT NULL
);


ALTER TABLE public.client_session_prot_mapper OWNER TO keycloak;

--
-- Name: client_session_role; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.client_session_role (
    role_id character varying(255) NOT NULL,
    client_session character varying(36) NOT NULL
);


ALTER TABLE public.client_session_role OWNER TO keycloak;

--
-- Name: client_user_session_note; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.client_user_session_note (
    name character varying(255) NOT NULL,
    value character varying(2048),
    client_session character varying(36) NOT NULL
);


ALTER TABLE public.client_user_session_note OWNER TO keycloak;

--
-- Name: component; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.component (
    id character varying(36) NOT NULL,
    name character varying(255),
    parent_id character varying(36),
    provider_id character varying(36),
    provider_type character varying(255),
    realm_id character varying(36),
    sub_type character varying(255)
);


ALTER TABLE public.component OWNER TO keycloak;

--
-- Name: component_config; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.component_config (
    id character varying(36) NOT NULL,
    component_id character varying(36) NOT NULL,
    name character varying(255) NOT NULL,
    value character varying(4000)
);


ALTER TABLE public.component_config OWNER TO keycloak;

--
-- Name: composite_role; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.composite_role (
    composite character varying(36) NOT NULL,
    child_role character varying(36) NOT NULL
);


ALTER TABLE public.composite_role OWNER TO keycloak;

--
-- Name: credential; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.credential (
    id character varying(36) NOT NULL,
    salt bytea,
    type character varying(255),
    user_id character varying(36),
    created_date bigint,
    user_label character varying(255),
    secret_data text,
    credential_data text,
    priority integer
);


ALTER TABLE public.credential OWNER TO keycloak;

--
-- Name: databasechangelog; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.databasechangelog (
    id character varying(255) NOT NULL,
    author character varying(255) NOT NULL,
    filename character varying(255) NOT NULL,
    dateexecuted timestamp without time zone NOT NULL,
    orderexecuted integer NOT NULL,
    exectype character varying(10) NOT NULL,
    md5sum character varying(35),
    description character varying(255),
    comments character varying(255),
    tag character varying(255),
    liquibase character varying(20),
    contexts character varying(255),
    labels character varying(255),
    deployment_id character varying(10)
);


ALTER TABLE public.databasechangelog OWNER TO keycloak;

--
-- Name: databasechangeloglock; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.databasechangeloglock (
    id integer NOT NULL,
    locked boolean NOT NULL,
    lockgranted timestamp without time zone,
    lockedby character varying(255)
);


ALTER TABLE public.databasechangeloglock OWNER TO keycloak;

--
-- Name: default_client_scope; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.default_client_scope (
    realm_id character varying(36) NOT NULL,
    scope_id character varying(36) NOT NULL,
    default_scope boolean DEFAULT false NOT NULL
);


ALTER TABLE public.default_client_scope OWNER TO keycloak;

--
-- Name: event_entity; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.event_entity (
    id character varying(36) NOT NULL,
    client_id character varying(255),
    details_json character varying(2550),
    error character varying(255),
    ip_address character varying(255),
    realm_id character varying(255),
    session_id character varying(255),
    event_time bigint,
    type character varying(255),
    user_id character varying(255)
);


ALTER TABLE public.event_entity OWNER TO keycloak;

--
-- Name: fed_user_attribute; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.fed_user_attribute (
    id character varying(36) NOT NULL,
    name character varying(255) NOT NULL,
    user_id character varying(255) NOT NULL,
    realm_id character varying(36) NOT NULL,
    storage_provider_id character varying(36),
    value character varying(2024)
);


ALTER TABLE public.fed_user_attribute OWNER TO keycloak;

--
-- Name: fed_user_consent; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.fed_user_consent (
    id character varying(36) NOT NULL,
    client_id character varying(36),
    user_id character varying(255) NOT NULL,
    realm_id character varying(36) NOT NULL,
    storage_provider_id character varying(36),
    created_date bigint,
    last_updated_date bigint,
    client_storage_provider character varying(36),
    external_client_id character varying(255)
);


ALTER TABLE public.fed_user_consent OWNER TO keycloak;

--
-- Name: fed_user_consent_cl_scope; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.fed_user_consent_cl_scope (
    user_consent_id character varying(36) NOT NULL,
    scope_id character varying(36) NOT NULL
);


ALTER TABLE public.fed_user_consent_cl_scope OWNER TO keycloak;

--
-- Name: fed_user_credential; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.fed_user_credential (
    id character varying(36) NOT NULL,
    salt bytea,
    type character varying(255),
    created_date bigint,
    user_id character varying(255) NOT NULL,
    realm_id character varying(36) NOT NULL,
    storage_provider_id character varying(36),
    user_label character varying(255),
    secret_data text,
    credential_data text,
    priority integer
);


ALTER TABLE public.fed_user_credential OWNER TO keycloak;

--
-- Name: fed_user_group_membership; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.fed_user_group_membership (
    group_id character varying(36) NOT NULL,
    user_id character varying(255) NOT NULL,
    realm_id character varying(36) NOT NULL,
    storage_provider_id character varying(36)
);


ALTER TABLE public.fed_user_group_membership OWNER TO keycloak;

--
-- Name: fed_user_required_action; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.fed_user_required_action (
    required_action character varying(255) DEFAULT ' '::character varying NOT NULL,
    user_id character varying(255) NOT NULL,
    realm_id character varying(36) NOT NULL,
    storage_provider_id character varying(36)
);


ALTER TABLE public.fed_user_required_action OWNER TO keycloak;

--
-- Name: fed_user_role_mapping; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.fed_user_role_mapping (
    role_id character varying(36) NOT NULL,
    user_id character varying(255) NOT NULL,
    realm_id character varying(36) NOT NULL,
    storage_provider_id character varying(36)
);


ALTER TABLE public.fed_user_role_mapping OWNER TO keycloak;

--
-- Name: federated_identity; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.federated_identity (
    identity_provider character varying(255) NOT NULL,
    realm_id character varying(36),
    federated_user_id character varying(255),
    federated_username character varying(255),
    token text,
    user_id character varying(36) NOT NULL
);


ALTER TABLE public.federated_identity OWNER TO keycloak;

--
-- Name: federated_user; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.federated_user (
    id character varying(255) NOT NULL,
    storage_provider_id character varying(255),
    realm_id character varying(36) NOT NULL
);


ALTER TABLE public.federated_user OWNER TO keycloak;

--
-- Name: group_attribute; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.group_attribute (
    id character varying(36) DEFAULT 'sybase-needs-something-here'::character varying NOT NULL,
    name character varying(255) NOT NULL,
    value character varying(255),
    group_id character varying(36) NOT NULL
);


ALTER TABLE public.group_attribute OWNER TO keycloak;

--
-- Name: group_role_mapping; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.group_role_mapping (
    role_id character varying(36) NOT NULL,
    group_id character varying(36) NOT NULL
);


ALTER TABLE public.group_role_mapping OWNER TO keycloak;

--
-- Name: identity_provider; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.identity_provider (
    internal_id character varying(36) NOT NULL,
    enabled boolean DEFAULT false NOT NULL,
    provider_alias character varying(255),
    provider_id character varying(255),
    store_token boolean DEFAULT false NOT NULL,
    authenticate_by_default boolean DEFAULT false NOT NULL,
    realm_id character varying(36),
    add_token_role boolean DEFAULT true NOT NULL,
    trust_email boolean DEFAULT false NOT NULL,
    first_broker_login_flow_id character varying(36),
    post_broker_login_flow_id character varying(36),
    provider_display_name character varying(255),
    link_only boolean DEFAULT false NOT NULL
);


ALTER TABLE public.identity_provider OWNER TO keycloak;

--
-- Name: identity_provider_config; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.identity_provider_config (
    identity_provider_id character varying(36) NOT NULL,
    value text,
    name character varying(255) NOT NULL
);


ALTER TABLE public.identity_provider_config OWNER TO keycloak;

--
-- Name: identity_provider_mapper; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.identity_provider_mapper (
    id character varying(36) NOT NULL,
    name character varying(255) NOT NULL,
    idp_alias character varying(255) NOT NULL,
    idp_mapper_name character varying(255) NOT NULL,
    realm_id character varying(36) NOT NULL
);


ALTER TABLE public.identity_provider_mapper OWNER TO keycloak;

--
-- Name: idp_mapper_config; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.idp_mapper_config (
    idp_mapper_id character varying(36) NOT NULL,
    value text,
    name character varying(255) NOT NULL
);


ALTER TABLE public.idp_mapper_config OWNER TO keycloak;

--
-- Name: keycloak_group; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.keycloak_group (
    id character varying(36) NOT NULL,
    name character varying(255),
    parent_group character varying(36),
    realm_id character varying(36)
);


ALTER TABLE public.keycloak_group OWNER TO keycloak;

--
-- Name: keycloak_role; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.keycloak_role (
    id character varying(36) NOT NULL,
    client_realm_constraint character varying(36),
    client_role boolean DEFAULT false NOT NULL,
    description character varying(255),
    name character varying(255),
    realm_id character varying(255),
    client character varying(36),
    realm character varying(36)
);


ALTER TABLE public.keycloak_role OWNER TO keycloak;

--
-- Name: migration_model; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.migration_model (
    id character varying(36) NOT NULL,
    version character varying(36),
    update_time bigint DEFAULT 0 NOT NULL
);


ALTER TABLE public.migration_model OWNER TO keycloak;

--
-- Name: offline_client_session; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.offline_client_session (
    user_session_id character varying(36) NOT NULL,
    client_id character varying(36) NOT NULL,
    offline_flag character varying(4) NOT NULL,
    "timestamp" integer,
    data text,
    client_storage_provider character varying(36) DEFAULT 'local'::character varying NOT NULL,
    external_client_id character varying(255) DEFAULT 'local'::character varying NOT NULL
);


ALTER TABLE public.offline_client_session OWNER TO keycloak;

--
-- Name: offline_user_session; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.offline_user_session (
    user_session_id character varying(36) NOT NULL,
    user_id character varying(255) NOT NULL,
    realm_id character varying(36) NOT NULL,
    created_on integer NOT NULL,
    offline_flag character varying(4) NOT NULL,
    data text,
    last_session_refresh integer DEFAULT 0 NOT NULL
);


ALTER TABLE public.offline_user_session OWNER TO keycloak;

--
-- Name: policy_config; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.policy_config (
    policy_id character varying(36) NOT NULL,
    name character varying(255) NOT NULL,
    value text
);


ALTER TABLE public.policy_config OWNER TO keycloak;

--
-- Name: protocol_mapper; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.protocol_mapper (
    id character varying(36) NOT NULL,
    name character varying(255) NOT NULL,
    protocol character varying(255) NOT NULL,
    protocol_mapper_name character varying(255) NOT NULL,
    client_id character varying(36),
    client_scope_id character varying(36)
);


ALTER TABLE public.protocol_mapper OWNER TO keycloak;

--
-- Name: protocol_mapper_config; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.protocol_mapper_config (
    protocol_mapper_id character varying(36) NOT NULL,
    value text,
    name character varying(255) NOT NULL
);


ALTER TABLE public.protocol_mapper_config OWNER TO keycloak;

--
-- Name: realm; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.realm (
    id character varying(36) NOT NULL,
    access_code_lifespan integer,
    user_action_lifespan integer,
    access_token_lifespan integer,
    account_theme character varying(255),
    admin_theme character varying(255),
    email_theme character varying(255),
    enabled boolean DEFAULT false NOT NULL,
    events_enabled boolean DEFAULT false NOT NULL,
    events_expiration bigint,
    login_theme character varying(255),
    name character varying(255),
    not_before integer,
    password_policy character varying(2550),
    registration_allowed boolean DEFAULT false NOT NULL,
    remember_me boolean DEFAULT false NOT NULL,
    reset_password_allowed boolean DEFAULT false NOT NULL,
    social boolean DEFAULT false NOT NULL,
    ssl_required character varying(255),
    sso_idle_timeout integer,
    sso_max_lifespan integer,
    update_profile_on_soc_login boolean DEFAULT false NOT NULL,
    verify_email boolean DEFAULT false NOT NULL,
    master_admin_client character varying(36),
    login_lifespan integer,
    internationalization_enabled boolean DEFAULT false NOT NULL,
    default_locale character varying(255),
    reg_email_as_username boolean DEFAULT false NOT NULL,
    admin_events_enabled boolean DEFAULT false NOT NULL,
    admin_events_details_enabled boolean DEFAULT false NOT NULL,
    edit_username_allowed boolean DEFAULT false NOT NULL,
    otp_policy_counter integer DEFAULT 0,
    otp_policy_window integer DEFAULT 1,
    otp_policy_period integer DEFAULT 30,
    otp_policy_digits integer DEFAULT 6,
    otp_policy_alg character varying(36) DEFAULT 'HmacSHA1'::character varying,
    otp_policy_type character varying(36) DEFAULT 'totp'::character varying,
    browser_flow character varying(36),
    registration_flow character varying(36),
    direct_grant_flow character varying(36),
    reset_credentials_flow character varying(36),
    client_auth_flow character varying(36),
    offline_session_idle_timeout integer DEFAULT 0,
    revoke_refresh_token boolean DEFAULT false NOT NULL,
    access_token_life_implicit integer DEFAULT 0,
    login_with_email_allowed boolean DEFAULT true NOT NULL,
    duplicate_emails_allowed boolean DEFAULT false NOT NULL,
    docker_auth_flow character varying(36),
    refresh_token_max_reuse integer DEFAULT 0,
    allow_user_managed_access boolean DEFAULT false NOT NULL,
    sso_max_lifespan_remember_me integer DEFAULT 0 NOT NULL,
    sso_idle_timeout_remember_me integer DEFAULT 0 NOT NULL
);


ALTER TABLE public.realm OWNER TO keycloak;

--
-- Name: realm_attribute; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.realm_attribute (
    name character varying(255) NOT NULL,
    value character varying(255),
    realm_id character varying(36) NOT NULL
);


ALTER TABLE public.realm_attribute OWNER TO keycloak;

--
-- Name: realm_default_groups; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.realm_default_groups (
    realm_id character varying(36) NOT NULL,
    group_id character varying(36) NOT NULL
);


ALTER TABLE public.realm_default_groups OWNER TO keycloak;

--
-- Name: realm_default_roles; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.realm_default_roles (
    realm_id character varying(36) NOT NULL,
    role_id character varying(36) NOT NULL
);


ALTER TABLE public.realm_default_roles OWNER TO keycloak;

--
-- Name: realm_enabled_event_types; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.realm_enabled_event_types (
    realm_id character varying(36) NOT NULL,
    value character varying(255) NOT NULL
);


ALTER TABLE public.realm_enabled_event_types OWNER TO keycloak;

--
-- Name: realm_events_listeners; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.realm_events_listeners (
    realm_id character varying(36) NOT NULL,
    value character varying(255) NOT NULL
);


ALTER TABLE public.realm_events_listeners OWNER TO keycloak;

--
-- Name: realm_required_credential; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.realm_required_credential (
    type character varying(255) NOT NULL,
    form_label character varying(255),
    input boolean DEFAULT false NOT NULL,
    secret boolean DEFAULT false NOT NULL,
    realm_id character varying(36) NOT NULL
);


ALTER TABLE public.realm_required_credential OWNER TO keycloak;

--
-- Name: realm_smtp_config; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.realm_smtp_config (
    realm_id character varying(36) NOT NULL,
    value character varying(255),
    name character varying(255) NOT NULL
);


ALTER TABLE public.realm_smtp_config OWNER TO keycloak;

--
-- Name: realm_supported_locales; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.realm_supported_locales (
    realm_id character varying(36) NOT NULL,
    value character varying(255) NOT NULL
);


ALTER TABLE public.realm_supported_locales OWNER TO keycloak;

--
-- Name: redirect_uris; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.redirect_uris (
    client_id character varying(36) NOT NULL,
    value character varying(255) NOT NULL
);


ALTER TABLE public.redirect_uris OWNER TO keycloak;

--
-- Name: required_action_config; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.required_action_config (
    required_action_id character varying(36) NOT NULL,
    value text,
    name character varying(255) NOT NULL
);


ALTER TABLE public.required_action_config OWNER TO keycloak;

--
-- Name: required_action_provider; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.required_action_provider (
    id character varying(36) NOT NULL,
    alias character varying(255),
    name character varying(255),
    realm_id character varying(36),
    enabled boolean DEFAULT false NOT NULL,
    default_action boolean DEFAULT false NOT NULL,
    provider_id character varying(255),
    priority integer
);


ALTER TABLE public.required_action_provider OWNER TO keycloak;

--
-- Name: resource_attribute; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.resource_attribute (
    id character varying(36) DEFAULT 'sybase-needs-something-here'::character varying NOT NULL,
    name character varying(255) NOT NULL,
    value character varying(255),
    resource_id character varying(36) NOT NULL
);


ALTER TABLE public.resource_attribute OWNER TO keycloak;

--
-- Name: resource_policy; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.resource_policy (
    resource_id character varying(36) NOT NULL,
    policy_id character varying(36) NOT NULL
);


ALTER TABLE public.resource_policy OWNER TO keycloak;

--
-- Name: resource_scope; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.resource_scope (
    resource_id character varying(36) NOT NULL,
    scope_id character varying(36) NOT NULL
);


ALTER TABLE public.resource_scope OWNER TO keycloak;

--
-- Name: resource_server; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.resource_server (
    id character varying(36) NOT NULL,
    allow_rs_remote_mgmt boolean DEFAULT false NOT NULL,
    policy_enforce_mode character varying(15) NOT NULL,
    decision_strategy smallint DEFAULT 1 NOT NULL
);


ALTER TABLE public.resource_server OWNER TO keycloak;

--
-- Name: resource_server_perm_ticket; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.resource_server_perm_ticket (
    id character varying(36) NOT NULL,
    owner character varying(36) NOT NULL,
    requester character varying(36) NOT NULL,
    created_timestamp bigint NOT NULL,
    granted_timestamp bigint,
    resource_id character varying(36) NOT NULL,
    scope_id character varying(36),
    resource_server_id character varying(36) NOT NULL,
    policy_id character varying(36)
);


ALTER TABLE public.resource_server_perm_ticket OWNER TO keycloak;

--
-- Name: resource_server_policy; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.resource_server_policy (
    id character varying(36) NOT NULL,
    name character varying(255) NOT NULL,
    description character varying(255),
    type character varying(255) NOT NULL,
    decision_strategy character varying(20),
    logic character varying(20),
    resource_server_id character varying(36) NOT NULL,
    owner character varying(36)
);


ALTER TABLE public.resource_server_policy OWNER TO keycloak;

--
-- Name: resource_server_resource; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.resource_server_resource (
    id character varying(36) NOT NULL,
    name character varying(255) NOT NULL,
    type character varying(255),
    icon_uri character varying(255),
    owner character varying(36) NOT NULL,
    resource_server_id character varying(36) NOT NULL,
    owner_managed_access boolean DEFAULT false NOT NULL,
    display_name character varying(255)
);


ALTER TABLE public.resource_server_resource OWNER TO keycloak;

--
-- Name: resource_server_scope; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.resource_server_scope (
    id character varying(36) NOT NULL,
    name character varying(255) NOT NULL,
    icon_uri character varying(255),
    resource_server_id character varying(36) NOT NULL,
    display_name character varying(255)
);


ALTER TABLE public.resource_server_scope OWNER TO keycloak;

--
-- Name: resource_uris; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.resource_uris (
    resource_id character varying(36) NOT NULL,
    value character varying(255) NOT NULL
);


ALTER TABLE public.resource_uris OWNER TO keycloak;

--
-- Name: role_attribute; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.role_attribute (
    id character varying(36) NOT NULL,
    role_id character varying(36) NOT NULL,
    name character varying(255) NOT NULL,
    value character varying(255)
);


ALTER TABLE public.role_attribute OWNER TO keycloak;

--
-- Name: scope_mapping; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.scope_mapping (
    client_id character varying(36) NOT NULL,
    role_id character varying(36) NOT NULL
);


ALTER TABLE public.scope_mapping OWNER TO keycloak;

--
-- Name: scope_policy; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.scope_policy (
    scope_id character varying(36) NOT NULL,
    policy_id character varying(36) NOT NULL
);


ALTER TABLE public.scope_policy OWNER TO keycloak;

--
-- Name: user_attribute; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.user_attribute (
    name character varying(255) NOT NULL,
    value character varying(255),
    user_id character varying(36) NOT NULL,
    id character varying(36) DEFAULT 'sybase-needs-something-here'::character varying NOT NULL
);


ALTER TABLE public.user_attribute OWNER TO keycloak;

--
-- Name: user_consent; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.user_consent (
    id character varying(36) NOT NULL,
    client_id character varying(36),
    user_id character varying(36) NOT NULL,
    created_date bigint,
    last_updated_date bigint,
    client_storage_provider character varying(36),
    external_client_id character varying(255)
);


ALTER TABLE public.user_consent OWNER TO keycloak;

--
-- Name: user_consent_client_scope; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.user_consent_client_scope (
    user_consent_id character varying(36) NOT NULL,
    scope_id character varying(36) NOT NULL
);


ALTER TABLE public.user_consent_client_scope OWNER TO keycloak;

--
-- Name: user_entity; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.user_entity (
    id character varying(36) NOT NULL,
    email character varying(255),
    email_constraint character varying(255),
    email_verified boolean DEFAULT false NOT NULL,
    enabled boolean DEFAULT false NOT NULL,
    federation_link character varying(255),
    first_name character varying(255),
    last_name character varying(255),
    realm_id character varying(255),
    username character varying(255),
    created_timestamp bigint,
    service_account_client_link character varying(36),
    not_before integer DEFAULT 0 NOT NULL
);


ALTER TABLE public.user_entity OWNER TO keycloak;

--
-- Name: user_federation_config; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.user_federation_config (
    user_federation_provider_id character varying(36) NOT NULL,
    value character varying(255),
    name character varying(255) NOT NULL
);


ALTER TABLE public.user_federation_config OWNER TO keycloak;

--
-- Name: user_federation_mapper; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.user_federation_mapper (
    id character varying(36) NOT NULL,
    name character varying(255) NOT NULL,
    federation_provider_id character varying(36) NOT NULL,
    federation_mapper_type character varying(255) NOT NULL,
    realm_id character varying(36) NOT NULL
);


ALTER TABLE public.user_federation_mapper OWNER TO keycloak;

--
-- Name: user_federation_mapper_config; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.user_federation_mapper_config (
    user_federation_mapper_id character varying(36) NOT NULL,
    value character varying(255),
    name character varying(255) NOT NULL
);


ALTER TABLE public.user_federation_mapper_config OWNER TO keycloak;

--
-- Name: user_federation_provider; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.user_federation_provider (
    id character varying(36) NOT NULL,
    changed_sync_period integer,
    display_name character varying(255),
    full_sync_period integer,
    last_sync integer,
    priority integer,
    provider_name character varying(255),
    realm_id character varying(36)
);


ALTER TABLE public.user_federation_provider OWNER TO keycloak;

--
-- Name: user_group_membership; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.user_group_membership (
    group_id character varying(36) NOT NULL,
    user_id character varying(36) NOT NULL
);


ALTER TABLE public.user_group_membership OWNER TO keycloak;

--
-- Name: user_required_action; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.user_required_action (
    user_id character varying(36) NOT NULL,
    required_action character varying(255) DEFAULT ' '::character varying NOT NULL
);


ALTER TABLE public.user_required_action OWNER TO keycloak;

--
-- Name: user_role_mapping; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.user_role_mapping (
    role_id character varying(255) NOT NULL,
    user_id character varying(36) NOT NULL
);


ALTER TABLE public.user_role_mapping OWNER TO keycloak;

--
-- Name: user_session; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.user_session (
    id character varying(36) NOT NULL,
    auth_method character varying(255),
    ip_address character varying(255),
    last_session_refresh integer,
    login_username character varying(255),
    realm_id character varying(255),
    remember_me boolean DEFAULT false NOT NULL,
    started integer,
    user_id character varying(255),
    user_session_state integer,
    broker_session_id character varying(255),
    broker_user_id character varying(255)
);


ALTER TABLE public.user_session OWNER TO keycloak;

--
-- Name: user_session_note; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.user_session_note (
    user_session character varying(36) NOT NULL,
    name character varying(255) NOT NULL,
    value character varying(2048)
);


ALTER TABLE public.user_session_note OWNER TO keycloak;

--
-- Name: username_login_failure; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.username_login_failure (
    realm_id character varying(36) NOT NULL,
    username character varying(255) NOT NULL,
    failed_login_not_before integer,
    last_failure bigint,
    last_ip_failure character varying(255),
    num_failures integer
);


ALTER TABLE public.username_login_failure OWNER TO keycloak;

--
-- Name: web_origins; Type: TABLE; Schema: public; Owner: keycloak
--

CREATE TABLE public.web_origins (
    client_id character varying(36) NOT NULL,
    value character varying(255) NOT NULL
);


ALTER TABLE public.web_origins OWNER TO keycloak;

--
-- Data for Name: admin_event_entity; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.admin_event_entity (id, admin_event_time, realm_id, operation_type, auth_realm_id, auth_client_id, auth_user_id, ip_address, resource_path, representation, error, resource_type) FROM stdin;
\.


--
-- Data for Name: associated_policy; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.associated_policy (policy_id, associated_policy_id) FROM stdin;
2fa44c63-4ea8-4438-a6fa-2b7f4306a307	22d389b9-62a8-481e-87be-7d38f901a574
\.


--
-- Data for Name: authentication_execution; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.authentication_execution (id, alias, authenticator, realm_id, flow_id, requirement, priority, authenticator_flow, auth_flow_id, auth_config) FROM stdin;
54d03a4a-e51b-465c-8109-cc468e069917	\N	auth-cookie	master	95b9a29d-b501-4615-8c9b-84968b2e8a03	2	10	f	\N	\N
9657e2a2-b617-474a-922b-e10c4340f225	\N	auth-spnego	master	95b9a29d-b501-4615-8c9b-84968b2e8a03	3	20	f	\N	\N
021ae582-6048-4e19-be62-946bf61481b8	\N	identity-provider-redirector	master	95b9a29d-b501-4615-8c9b-84968b2e8a03	2	25	f	\N	\N
f0c5e8e4-ba0e-4e1a-a81b-b8cfcb001826	\N	\N	master	95b9a29d-b501-4615-8c9b-84968b2e8a03	2	30	t	b68fa336-ebc5-41cd-b743-351600d5bab7	\N
f59581fa-8a7a-4db4-8afe-fb68d2672b3b	\N	auth-username-password-form	master	b68fa336-ebc5-41cd-b743-351600d5bab7	0	10	f	\N	\N
b68add06-a397-4b68-b8d2-84a5b8530160	\N	\N	master	b68fa336-ebc5-41cd-b743-351600d5bab7	1	20	t	40f651bc-440e-4e45-ae2d-71fb4a0e1c74	\N
7f37d4ba-4d11-40a7-b756-3789a7f3e506	\N	conditional-user-configured	master	40f651bc-440e-4e45-ae2d-71fb4a0e1c74	0	10	f	\N	\N
24c14cc9-c553-4e9a-806e-f214c60f9767	\N	auth-otp-form	master	40f651bc-440e-4e45-ae2d-71fb4a0e1c74	0	20	f	\N	\N
70d2031c-3979-4d3b-bdd2-e9c67f15dffb	\N	direct-grant-validate-username	master	b66f38b7-9cc5-42c9-a721-c0b33578b7ce	0	10	f	\N	\N
13e7fe12-b289-45de-b8b4-f011c7c331ad	\N	direct-grant-validate-password	master	b66f38b7-9cc5-42c9-a721-c0b33578b7ce	0	20	f	\N	\N
a5facb83-5282-4f92-8de6-95593b2c63df	\N	\N	master	b66f38b7-9cc5-42c9-a721-c0b33578b7ce	1	30	t	f02f44f4-5d63-43fd-89fa-d90abd9bfdc4	\N
3042a497-e91f-460f-a988-78907e879c3c	\N	conditional-user-configured	master	f02f44f4-5d63-43fd-89fa-d90abd9bfdc4	0	10	f	\N	\N
057089f0-59f9-489d-9e50-0a75ae622f3e	\N	direct-grant-validate-otp	master	f02f44f4-5d63-43fd-89fa-d90abd9bfdc4	0	20	f	\N	\N
a2109f22-e7bd-4c4a-8709-7998c461d8a3	\N	registration-page-form	master	eccd353b-9457-4eb0-b750-0917d1157ddd	0	10	t	f7c52492-70e0-47e9-94a4-faf0799f9046	\N
9e79faa3-fa13-4d5b-855e-9ee14c3bf851	\N	registration-user-creation	master	f7c52492-70e0-47e9-94a4-faf0799f9046	0	20	f	\N	\N
9c661113-4dc7-4217-bd51-da2d33129ba2	\N	registration-profile-action	master	f7c52492-70e0-47e9-94a4-faf0799f9046	0	40	f	\N	\N
061ad07d-6b50-48b9-b875-b1d79c7c68d5	\N	registration-password-action	master	f7c52492-70e0-47e9-94a4-faf0799f9046	0	50	f	\N	\N
b166377d-8925-47b0-99f0-a1c32a0afaaf	\N	registration-recaptcha-action	master	f7c52492-70e0-47e9-94a4-faf0799f9046	3	60	f	\N	\N
5fffb6a8-94ee-4f2b-94b7-38bcd091169c	\N	reset-credentials-choose-user	master	3545f14e-f248-4ec2-93d8-da885e104de0	0	10	f	\N	\N
af8281da-5858-4f31-9b6f-9026148424ec	\N	reset-credential-email	master	3545f14e-f248-4ec2-93d8-da885e104de0	0	20	f	\N	\N
80b1b7db-d996-4987-b769-dde26dfe73a6	\N	reset-password	master	3545f14e-f248-4ec2-93d8-da885e104de0	0	30	f	\N	\N
98a1fda0-f09b-40e9-8648-a884d134481e	\N	\N	master	3545f14e-f248-4ec2-93d8-da885e104de0	1	40	t	34d0a086-efb0-4734-9843-92d88458e2df	\N
485d02d9-2b42-40de-acde-bbb58e4f7270	\N	conditional-user-configured	master	34d0a086-efb0-4734-9843-92d88458e2df	0	10	f	\N	\N
b2418945-5eda-40a3-b82e-d71490e9bec7	\N	reset-otp	master	34d0a086-efb0-4734-9843-92d88458e2df	0	20	f	\N	\N
6eaa4e8a-09f6-418f-a48e-4cb588c12859	\N	client-secret	master	e769c1d6-210e-4885-bda1-2f8860c06cc9	2	10	f	\N	\N
c0239095-b2c9-4c98-b8cd-8f51f7b505c3	\N	client-jwt	master	e769c1d6-210e-4885-bda1-2f8860c06cc9	2	20	f	\N	\N
04dc7773-1bc3-4ce9-9e56-30346cfa0054	\N	client-secret-jwt	master	e769c1d6-210e-4885-bda1-2f8860c06cc9	2	30	f	\N	\N
ad090662-c69a-4083-9bee-4012b0f78a33	\N	client-x509	master	e769c1d6-210e-4885-bda1-2f8860c06cc9	2	40	f	\N	\N
989339c0-1806-4d64-8dbc-67b1302ab492	\N	idp-review-profile	master	e2c31d05-240c-45a9-9987-a9ed224a440a	0	10	f	\N	6612db44-5426-4335-b2c0-d08da082da9a
ae236a0d-a950-4f22-81e0-42b35ded1084	\N	\N	master	e2c31d05-240c-45a9-9987-a9ed224a440a	0	20	t	b5bb235f-f3c7-483e-bea4-133e67c9afa1	\N
2f007114-1c9b-41be-94af-a15408a8eb9f	\N	idp-create-user-if-unique	master	b5bb235f-f3c7-483e-bea4-133e67c9afa1	2	10	f	\N	bf6d0875-1986-46cc-926e-59f31d00e514
20767359-e4de-4c10-a8aa-6346c2827a1f	\N	\N	master	b5bb235f-f3c7-483e-bea4-133e67c9afa1	2	20	t	81e1de32-b1d7-40fd-9657-eb7a669291b0	\N
4a368095-eea8-4cd3-b209-40f9cdfcf66b	\N	idp-confirm-link	master	81e1de32-b1d7-40fd-9657-eb7a669291b0	0	10	f	\N	\N
2190c319-8875-4425-8e93-d3672587dec3	\N	\N	master	81e1de32-b1d7-40fd-9657-eb7a669291b0	0	20	t	84ef8d04-9a3f-43c7-9d6d-188f3e1fadf1	\N
9e1547ea-77dd-40b5-aed3-84be89877e68	\N	idp-email-verification	master	84ef8d04-9a3f-43c7-9d6d-188f3e1fadf1	2	10	f	\N	\N
f91cbcec-76d5-4bb0-8240-7b5eb58ce7ab	\N	\N	master	84ef8d04-9a3f-43c7-9d6d-188f3e1fadf1	2	20	t	e52473b5-353c-4f70-890f-5821e9e9602b	\N
c06acd1d-38af-4447-a129-5644818eea9b	\N	idp-username-password-form	master	e52473b5-353c-4f70-890f-5821e9e9602b	0	10	f	\N	\N
a2d81458-113a-44a4-9f62-a00a06a7cfb9	\N	\N	master	e52473b5-353c-4f70-890f-5821e9e9602b	1	20	t	d06d3ff7-b948-4910-aaa5-5feef02e5631	\N
6fd7aedc-96be-4b47-ade3-00818cb5ed8e	\N	conditional-user-configured	master	d06d3ff7-b948-4910-aaa5-5feef02e5631	0	10	f	\N	\N
333ea6fc-6190-4a48-ac3d-e20b98da8694	\N	auth-otp-form	master	d06d3ff7-b948-4910-aaa5-5feef02e5631	0	20	f	\N	\N
790309bc-b248-483d-ab29-27e0449c4b72	\N	http-basic-authenticator	master	0ef5c3fb-7f65-4032-aa12-43fc4e5c6605	0	10	f	\N	\N
eee69bac-8f69-4123-bd0b-f21361ba0cd2	\N	docker-http-basic-authenticator	master	dfea654f-225b-4da7-a0e3-de19cc7eb478	0	10	f	\N	\N
11f30eb5-cbbe-4a3e-a06d-e280cd58ede8	\N	no-cookie-redirect	master	643019b9-83ab-469f-b15c-4268206287f1	0	10	f	\N	\N
8462cb11-eff8-4a2c-8d76-a9ae44bd859b	\N	\N	master	643019b9-83ab-469f-b15c-4268206287f1	0	20	t	7a5023f8-8b9d-4083-ad15-0d13c5c8989c	\N
c2aab825-f30f-4d10-9615-1bbbf0f929ef	\N	basic-auth	master	7a5023f8-8b9d-4083-ad15-0d13c5c8989c	0	10	f	\N	\N
2ca2d9de-8b44-406e-af8c-0cd1be961f11	\N	basic-auth-otp	master	7a5023f8-8b9d-4083-ad15-0d13c5c8989c	3	20	f	\N	\N
cc006b36-2149-4ec3-975e-bc940fddf708	\N	auth-spnego	master	7a5023f8-8b9d-4083-ad15-0d13c5c8989c	3	30	f	\N	\N
5249c1fd-f6b7-4151-96a8-c9908ab2fe62	\N	auth-cookie	ModMappings	a04d0d12-d12f-4fc3-956b-94a95cf4d377	2	10	f	\N	\N
8e3d57f3-2ff2-4008-985c-a68532b41592	\N	auth-spnego	ModMappings	a04d0d12-d12f-4fc3-956b-94a95cf4d377	3	20	f	\N	\N
f3f28c92-b28e-45c4-b1f6-65bae2e1d1b3	\N	identity-provider-redirector	ModMappings	a04d0d12-d12f-4fc3-956b-94a95cf4d377	2	25	f	\N	\N
8412f4b2-fcdb-4f27-94a8-489b443293a1	\N	\N	ModMappings	a04d0d12-d12f-4fc3-956b-94a95cf4d377	2	30	t	d29db8b7-cb77-4a7a-bc98-1b5b93402425	\N
d5440b0b-8c7d-4b1b-ae9c-b68c7c7bf628	\N	auth-username-password-form	ModMappings	d29db8b7-cb77-4a7a-bc98-1b5b93402425	0	10	f	\N	\N
a7f3a998-43b8-47c2-b3c6-e316a7abddbd	\N	\N	ModMappings	d29db8b7-cb77-4a7a-bc98-1b5b93402425	1	20	t	c9d56f9f-83bf-46a6-aaba-aa1cd2c27394	\N
ce27273f-37e7-41ed-b3b0-3e3d58c43f94	\N	conditional-user-configured	ModMappings	c9d56f9f-83bf-46a6-aaba-aa1cd2c27394	0	10	f	\N	\N
52dd9d69-0dbc-405c-965a-b773b7333156	\N	auth-otp-form	ModMappings	c9d56f9f-83bf-46a6-aaba-aa1cd2c27394	0	20	f	\N	\N
8dadced8-eeb4-4ee8-9ac3-b8e00e6da2e3	\N	direct-grant-validate-username	ModMappings	110d53fe-16d9-4d94-8f43-f11598617f73	0	10	f	\N	\N
65636267-a132-474c-ba7b-2ff0d80b1799	\N	direct-grant-validate-password	ModMappings	110d53fe-16d9-4d94-8f43-f11598617f73	0	20	f	\N	\N
ecb43391-17a1-4be3-b466-a83171c62c3f	\N	\N	ModMappings	110d53fe-16d9-4d94-8f43-f11598617f73	1	30	t	eb9db164-dfd0-47f8-ad6f-ded0d67a3b67	\N
79cdff0a-d61a-43c0-bca8-08486314fc03	\N	conditional-user-configured	ModMappings	eb9db164-dfd0-47f8-ad6f-ded0d67a3b67	0	10	f	\N	\N
189f10a4-60ff-4f51-8d60-82bc7632c6e0	\N	direct-grant-validate-otp	ModMappings	eb9db164-dfd0-47f8-ad6f-ded0d67a3b67	0	20	f	\N	\N
2bd83874-0980-4560-8596-1d1c33b95c5d	\N	registration-page-form	ModMappings	05b34418-8d4f-4bf7-8e6e-e34cd3e2dc45	0	10	t	db7c1377-4c5e-4031-9d7d-d91f7f55513a	\N
7179d5cb-cf47-4c41-80bd-e2c70db4604c	\N	registration-user-creation	ModMappings	db7c1377-4c5e-4031-9d7d-d91f7f55513a	0	20	f	\N	\N
0267a35c-c2e2-486f-a054-a086bab1067c	\N	registration-profile-action	ModMappings	db7c1377-4c5e-4031-9d7d-d91f7f55513a	0	40	f	\N	\N
661a9d18-2b46-43ad-8236-2680d6256902	\N	registration-password-action	ModMappings	db7c1377-4c5e-4031-9d7d-d91f7f55513a	0	50	f	\N	\N
2cf8b2b0-f463-40ed-99c4-dcba3e142dc0	\N	registration-recaptcha-action	ModMappings	db7c1377-4c5e-4031-9d7d-d91f7f55513a	3	60	f	\N	\N
94b6fb0b-8a50-4feb-9a79-b65a5eae01ce	\N	reset-credentials-choose-user	ModMappings	f6c01ce9-afac-4a85-adf5-3e67f0bd0188	0	10	f	\N	\N
865c7cc0-a28b-4b50-97ec-614e09776ffe	\N	reset-credential-email	ModMappings	f6c01ce9-afac-4a85-adf5-3e67f0bd0188	0	20	f	\N	\N
72e89c33-749b-4ad4-af4f-10605d1cad11	\N	reset-password	ModMappings	f6c01ce9-afac-4a85-adf5-3e67f0bd0188	0	30	f	\N	\N
8752e207-50c6-4ea4-b722-127208deceb8	\N	\N	ModMappings	f6c01ce9-afac-4a85-adf5-3e67f0bd0188	1	40	t	a17667ed-b65a-4055-8204-76dd72565611	\N
3a62a971-3060-4915-88db-fbaa95c6c49b	\N	conditional-user-configured	ModMappings	a17667ed-b65a-4055-8204-76dd72565611	0	10	f	\N	\N
20859e57-9661-452b-b840-8284169ce431	\N	reset-otp	ModMappings	a17667ed-b65a-4055-8204-76dd72565611	0	20	f	\N	\N
546e4cbd-3950-472c-8695-e30c67e659f4	\N	client-secret	ModMappings	15269a41-3f30-4bbb-9491-3107bf2ec42b	2	10	f	\N	\N
fecbe2bc-2afb-41a2-a64f-f03b29465e3f	\N	client-jwt	ModMappings	15269a41-3f30-4bbb-9491-3107bf2ec42b	2	20	f	\N	\N
1d158ec7-dccb-496d-8c0b-66c8e92e426d	\N	client-secret-jwt	ModMappings	15269a41-3f30-4bbb-9491-3107bf2ec42b	2	30	f	\N	\N
ef039370-e81c-4b37-a87c-cea27b227b78	\N	client-x509	ModMappings	15269a41-3f30-4bbb-9491-3107bf2ec42b	2	40	f	\N	\N
7a7ebffa-125c-4286-b82d-18c58cdba842	\N	idp-review-profile	ModMappings	396afa30-5aeb-478c-bb5d-0b67ee23b367	0	10	f	\N	e0dfb5da-0f22-4848-81a0-1167d5904e6b
905a8636-3fec-410d-b6d5-293ae35bf98b	\N	\N	ModMappings	396afa30-5aeb-478c-bb5d-0b67ee23b367	0	20	t	179590b4-4ba6-4d63-86de-f72f8e86127f	\N
ddfd9b34-cedc-4855-9d2f-06fd37c8bc89	\N	idp-create-user-if-unique	ModMappings	179590b4-4ba6-4d63-86de-f72f8e86127f	2	10	f	\N	e09b6d82-f33f-412e-93bf-21d6c7746888
910801e8-7194-43d5-9577-d038e21d5e65	\N	\N	ModMappings	179590b4-4ba6-4d63-86de-f72f8e86127f	2	20	t	6a385257-a689-4b9c-b118-52bae264e92f	\N
64326c5e-c246-4648-9ec1-15c81df55eae	\N	idp-confirm-link	ModMappings	6a385257-a689-4b9c-b118-52bae264e92f	0	10	f	\N	\N
0f6f93b7-d17c-4b3a-868c-675af4431a56	\N	\N	ModMappings	6a385257-a689-4b9c-b118-52bae264e92f	0	20	t	aefaefd3-8b96-4138-863e-53b7b84313aa	\N
d477f4af-eeeb-4f16-80ae-a018e0bebcab	\N	idp-email-verification	ModMappings	aefaefd3-8b96-4138-863e-53b7b84313aa	2	10	f	\N	\N
1630c579-2dd0-49ae-b8cf-80fd873d5aa5	\N	\N	ModMappings	aefaefd3-8b96-4138-863e-53b7b84313aa	2	20	t	52d2309b-cdfc-4fee-987e-d602d80d22d3	\N
dfaba7c4-dd24-40e8-aa86-8b9360ec162e	\N	idp-username-password-form	ModMappings	52d2309b-cdfc-4fee-987e-d602d80d22d3	0	10	f	\N	\N
58773485-a0d4-486b-b982-d20bea6f0f2a	\N	\N	ModMappings	52d2309b-cdfc-4fee-987e-d602d80d22d3	1	20	t	78d40c53-1799-44a6-914d-d997ef51986b	\N
3988b53d-2aee-4b15-b3a3-475aaa6cc3d5	\N	conditional-user-configured	ModMappings	78d40c53-1799-44a6-914d-d997ef51986b	0	10	f	\N	\N
7ef5e37b-a8bb-4210-a504-d04268f54d82	\N	auth-otp-form	ModMappings	78d40c53-1799-44a6-914d-d997ef51986b	0	20	f	\N	\N
05bd8186-61b6-4c6c-92b7-66f63cdc1ff7	\N	http-basic-authenticator	ModMappings	57d3834a-ba78-436b-985c-0a29a0ccb373	0	10	f	\N	\N
2ce0f70a-027a-4fe7-9478-f69acf880650	\N	docker-http-basic-authenticator	ModMappings	7dda0288-0e27-431e-ba8c-5e66a411a92f	0	10	f	\N	\N
210849d4-51d6-4b27-b612-766ab16d054a	\N	no-cookie-redirect	ModMappings	33d63e15-ec78-49b0-8303-4b342db5e6cc	0	10	f	\N	\N
59b73139-3a90-4d1f-bcb3-fce5374e9ee4	\N	\N	ModMappings	33d63e15-ec78-49b0-8303-4b342db5e6cc	0	20	t	88f71f0d-9539-4ce7-b6c0-7a99da8c6acf	\N
bb7cf179-39a6-4d01-9542-33aa5c89ac00	\N	basic-auth	ModMappings	88f71f0d-9539-4ce7-b6c0-7a99da8c6acf	0	10	f	\N	\N
001798e4-eda9-4ba9-8602-39934298a639	\N	basic-auth-otp	ModMappings	88f71f0d-9539-4ce7-b6c0-7a99da8c6acf	3	20	f	\N	\N
70f4dd8e-74cf-43fd-9ff9-e78e831173d5	\N	auth-spnego	ModMappings	88f71f0d-9539-4ce7-b6c0-7a99da8c6acf	3	30	f	\N	\N
\.


--
-- Data for Name: authentication_flow; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.authentication_flow (id, alias, description, realm_id, provider_id, top_level, built_in) FROM stdin;
95b9a29d-b501-4615-8c9b-84968b2e8a03	browser	browser based authentication	master	basic-flow	t	t
b68fa336-ebc5-41cd-b743-351600d5bab7	forms	Username, password, otp and other auth forms.	master	basic-flow	f	t
40f651bc-440e-4e45-ae2d-71fb4a0e1c74	Browser - Conditional OTP	Flow to determine if the OTP is required for the authentication	master	basic-flow	f	t
b66f38b7-9cc5-42c9-a721-c0b33578b7ce	direct grant	OpenID Connect Resource Owner Grant	master	basic-flow	t	t
f02f44f4-5d63-43fd-89fa-d90abd9bfdc4	Direct Grant - Conditional OTP	Flow to determine if the OTP is required for the authentication	master	basic-flow	f	t
eccd353b-9457-4eb0-b750-0917d1157ddd	registration	registration flow	master	basic-flow	t	t
f7c52492-70e0-47e9-94a4-faf0799f9046	registration form	registration form	master	form-flow	f	t
3545f14e-f248-4ec2-93d8-da885e104de0	reset credentials	Reset credentials for a user if they forgot their password or something	master	basic-flow	t	t
34d0a086-efb0-4734-9843-92d88458e2df	Reset - Conditional OTP	Flow to determine if the OTP should be reset or not. Set to REQUIRED to force.	master	basic-flow	f	t
e769c1d6-210e-4885-bda1-2f8860c06cc9	clients	Base authentication for clients	master	client-flow	t	t
e2c31d05-240c-45a9-9987-a9ed224a440a	first broker login	Actions taken after first broker login with identity provider account, which is not yet linked to any Keycloak account	master	basic-flow	t	t
b5bb235f-f3c7-483e-bea4-133e67c9afa1	User creation or linking	Flow for the existing/non-existing user alternatives	master	basic-flow	f	t
81e1de32-b1d7-40fd-9657-eb7a669291b0	Handle Existing Account	Handle what to do if there is existing account with same email/username like authenticated identity provider	master	basic-flow	f	t
84ef8d04-9a3f-43c7-9d6d-188f3e1fadf1	Account verification options	Method with which to verity the existing account	master	basic-flow	f	t
e52473b5-353c-4f70-890f-5821e9e9602b	Verify Existing Account by Re-authentication	Reauthentication of existing account	master	basic-flow	f	t
d06d3ff7-b948-4910-aaa5-5feef02e5631	First broker login - Conditional OTP	Flow to determine if the OTP is required for the authentication	master	basic-flow	f	t
0ef5c3fb-7f65-4032-aa12-43fc4e5c6605	saml ecp	SAML ECP Profile Authentication Flow	master	basic-flow	t	t
dfea654f-225b-4da7-a0e3-de19cc7eb478	docker auth	Used by Docker clients to authenticate against the IDP	master	basic-flow	t	t
643019b9-83ab-469f-b15c-4268206287f1	http challenge	An authentication flow based on challenge-response HTTP Authentication Schemes	master	basic-flow	t	t
7a5023f8-8b9d-4083-ad15-0d13c5c8989c	Authentication Options	Authentication options.	master	basic-flow	f	t
a04d0d12-d12f-4fc3-956b-94a95cf4d377	browser	browser based authentication	ModMappings	basic-flow	t	t
d29db8b7-cb77-4a7a-bc98-1b5b93402425	forms	Username, password, otp and other auth forms.	ModMappings	basic-flow	f	t
c9d56f9f-83bf-46a6-aaba-aa1cd2c27394	Browser - Conditional OTP	Flow to determine if the OTP is required for the authentication	ModMappings	basic-flow	f	t
110d53fe-16d9-4d94-8f43-f11598617f73	direct grant	OpenID Connect Resource Owner Grant	ModMappings	basic-flow	t	t
eb9db164-dfd0-47f8-ad6f-ded0d67a3b67	Direct Grant - Conditional OTP	Flow to determine if the OTP is required for the authentication	ModMappings	basic-flow	f	t
05b34418-8d4f-4bf7-8e6e-e34cd3e2dc45	registration	registration flow	ModMappings	basic-flow	t	t
db7c1377-4c5e-4031-9d7d-d91f7f55513a	registration form	registration form	ModMappings	form-flow	f	t
f6c01ce9-afac-4a85-adf5-3e67f0bd0188	reset credentials	Reset credentials for a user if they forgot their password or something	ModMappings	basic-flow	t	t
a17667ed-b65a-4055-8204-76dd72565611	Reset - Conditional OTP	Flow to determine if the OTP should be reset or not. Set to REQUIRED to force.	ModMappings	basic-flow	f	t
15269a41-3f30-4bbb-9491-3107bf2ec42b	clients	Base authentication for clients	ModMappings	client-flow	t	t
396afa30-5aeb-478c-bb5d-0b67ee23b367	first broker login	Actions taken after first broker login with identity provider account, which is not yet linked to any Keycloak account	ModMappings	basic-flow	t	t
179590b4-4ba6-4d63-86de-f72f8e86127f	User creation or linking	Flow for the existing/non-existing user alternatives	ModMappings	basic-flow	f	t
6a385257-a689-4b9c-b118-52bae264e92f	Handle Existing Account	Handle what to do if there is existing account with same email/username like authenticated identity provider	ModMappings	basic-flow	f	t
aefaefd3-8b96-4138-863e-53b7b84313aa	Account verification options	Method with which to verity the existing account	ModMappings	basic-flow	f	t
52d2309b-cdfc-4fee-987e-d602d80d22d3	Verify Existing Account by Re-authentication	Reauthentication of existing account	ModMappings	basic-flow	f	t
78d40c53-1799-44a6-914d-d997ef51986b	First broker login - Conditional OTP	Flow to determine if the OTP is required for the authentication	ModMappings	basic-flow	f	t
57d3834a-ba78-436b-985c-0a29a0ccb373	saml ecp	SAML ECP Profile Authentication Flow	ModMappings	basic-flow	t	t
7dda0288-0e27-431e-ba8c-5e66a411a92f	docker auth	Used by Docker clients to authenticate against the IDP	ModMappings	basic-flow	t	t
33d63e15-ec78-49b0-8303-4b342db5e6cc	http challenge	An authentication flow based on challenge-response HTTP Authentication Schemes	ModMappings	basic-flow	t	t
88f71f0d-9539-4ce7-b6c0-7a99da8c6acf	Authentication Options	Authentication options.	ModMappings	basic-flow	f	t
\.


--
-- Data for Name: authenticator_config; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.authenticator_config (id, alias, realm_id) FROM stdin;
6612db44-5426-4335-b2c0-d08da082da9a	review profile config	master
bf6d0875-1986-46cc-926e-59f31d00e514	create unique user config	master
e0dfb5da-0f22-4848-81a0-1167d5904e6b	review profile config	ModMappings
e09b6d82-f33f-412e-93bf-21d6c7746888	create unique user config	ModMappings
\.


--
-- Data for Name: authenticator_config_entry; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.authenticator_config_entry (authenticator_id, value, name) FROM stdin;
6612db44-5426-4335-b2c0-d08da082da9a	missing	update.profile.on.first.login
bf6d0875-1986-46cc-926e-59f31d00e514	false	require.password.update.after.registration
e0dfb5da-0f22-4848-81a0-1167d5904e6b	missing	update.profile.on.first.login
e09b6d82-f33f-412e-93bf-21d6c7746888	false	require.password.update.after.registration
\.


--
-- Data for Name: broker_link; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.broker_link (identity_provider, storage_provider_id, realm_id, broker_user_id, broker_username, token, user_id) FROM stdin;
\.


--
-- Data for Name: client; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.client (id, enabled, full_scope_allowed, client_id, not_before, public_client, secret, base_url, bearer_only, management_url, surrogate_auth_required, realm_id, protocol, node_rereg_timeout, frontchannel_logout, consent_required, name, service_accounts_enabled, client_authenticator_type, root_url, description, registration_token, standard_flow_enabled, implicit_flow_enabled, direct_access_grants_enabled) FROM stdin;
6e4abf2f-cad7-4035-8014-f7b86990dee7	t	t	master-realm	0	f	a4ea172c-f914-4a04-8358-e7ba350ea7ed	\N	t	\N	f	master	\N	0	f	f	master Realm	f	client-secret	\N	\N	\N	t	f	f
c5f82688-a1d0-4d07-96f3-6dd303a5bd61	t	f	account	0	f	db1dc7af-263d-4542-acd5-6fb989aeb6eb	/realms/master/account/	f	\N	f	master	openid-connect	0	f	f	${client_account}	f	client-secret	${authBaseUrl}	\N	\N	t	f	f
e9c4b2a4-e48b-477a-b5f1-3e911fce718d	t	f	broker	0	f	3202f1a5-5cf1-4e8c-888f-76fd546d19c5	\N	f	\N	f	master	openid-connect	0	f	f	${client_broker}	f	client-secret	\N	\N	\N	t	f	f
24463fff-1058-4fc9-8c1e-9ecbcf7fa2e9	t	f	security-admin-console	0	t	9b0bf09f-2233-4c16-b9db-9790c125fb0a	/admin/master/console/	f	\N	f	master	openid-connect	0	f	f	${client_security-admin-console}	f	client-secret	${authAdminUrl}	\N	\N	t	f	f
bd3f6db1-5cd4-4be3-a107-367aa2e903dc	t	f	admin-cli	0	t	65d9549f-af1c-40f5-9c3e-5a5e2a747aea	\N	f	\N	f	master	openid-connect	0	f	f	${client_admin-cli}	f	client-secret	\N	\N	\N	f	f	t
b1781787-3740-49f7-9015-51485579c1e5	t	t	ModMappings-realm	0	f	a1d1baf2-1a02-418b-bbc1-587eee40bf23	\N	t	\N	f	master	\N	0	f	f	ModMappings Realm	f	client-secret	\N	\N	\N	t	f	f
d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	f	realm-management	0	f	1cc9f2a4-afe8-481d-ac24-ead42d253e18	\N	t	\N	f	ModMappings	openid-connect	0	f	f	${client_realm-management}	f	client-secret	\N	\N	\N	t	f	f
b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	t	f	account	0	f	9e384267-731d-42b0-9ac9-b898f9c13757	/realms/ModMappings/account/	f	\N	f	ModMappings	openid-connect	0	f	f	${client_account}	f	client-secret	${authBaseUrl}	\N	\N	t	f	f
f9564792-f154-4177-ac4c-0d47d2f4c026	t	f	broker	0	f	de770411-d5bc-4325-a40a-03fb6f48a196	\N	f	\N	f	ModMappings	openid-connect	0	f	f	${client_broker}	f	client-secret	\N	\N	\N	t	f	f
d8945854-74b3-460b-9089-00fbc8c14672	t	f	security-admin-console	0	t	2ad2f875-3e2f-42f9-9185-ad52a6045463	/admin/ModMappings/console/	f	\N	f	ModMappings	openid-connect	0	f	f	${client_security-admin-console}	f	client-secret	${authAdminUrl}	\N	\N	t	f	f
e9f267a6-0ae2-4756-b8f3-7bf926e1db94	t	f	admin-cli	0	t	1fd4db99-b5e4-47ee-a004-371d357c650d	\N	f	\N	f	ModMappings	openid-connect	0	f	f	${client_admin-cli}	f	client-secret	\N	\N	\N	f	f	t
aed69ab6-ca48-4b7f-a94d-629361b04bb8	t	t	api	0	f	cf178ffa-3bf8-4879-a2ad-b01e3006dc4a	/swagger-ui.html	f		f	ModMappings	openid-connect	-1	f	f	\N	t	client-secret	https://api.modmappings.org	\N	\N	t	t	t
\.


--
-- Data for Name: client_attributes; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.client_attributes (client_id, value, name) FROM stdin;
aed69ab6-ca48-4b7f-a94d-629361b04bb8	false	saml.server.signature
aed69ab6-ca48-4b7f-a94d-629361b04bb8	false	saml.server.signature.keyinfo.ext
aed69ab6-ca48-4b7f-a94d-629361b04bb8	false	saml.assertion.signature
aed69ab6-ca48-4b7f-a94d-629361b04bb8	false	saml.client.signature
aed69ab6-ca48-4b7f-a94d-629361b04bb8	false	saml.encrypt
aed69ab6-ca48-4b7f-a94d-629361b04bb8	false	saml.authnstatement
aed69ab6-ca48-4b7f-a94d-629361b04bb8	false	saml.onetimeuse.condition
aed69ab6-ca48-4b7f-a94d-629361b04bb8	false	saml_force_name_id_format
aed69ab6-ca48-4b7f-a94d-629361b04bb8	false	saml.multivalued.roles
aed69ab6-ca48-4b7f-a94d-629361b04bb8	false	saml.force.post.binding
aed69ab6-ca48-4b7f-a94d-629361b04bb8	false	exclude.session.state.from.auth.response
aed69ab6-ca48-4b7f-a94d-629361b04bb8	false	tls.client.certificate.bound.access.tokens
aed69ab6-ca48-4b7f-a94d-629361b04bb8	false	display.on.consent.screen
\.


--
-- Data for Name: client_auth_flow_bindings; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.client_auth_flow_bindings (client_id, flow_id, binding_name) FROM stdin;
\.


--
-- Data for Name: client_default_roles; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.client_default_roles (client_id, role_id) FROM stdin;
c5f82688-a1d0-4d07-96f3-6dd303a5bd61	f2413a59-2051-4fde-85a2-6c4b62d4d6b6
c5f82688-a1d0-4d07-96f3-6dd303a5bd61	7ffebb0e-73d8-4f0b-acee-44b682519130
b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	9e9c4679-a175-4a04-b72b-234686a133b6
b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	625aa352-faf9-4bfe-99f1-5f270200eb75
\.


--
-- Data for Name: client_initial_access; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.client_initial_access (id, realm_id, "timestamp", expiration, count, remaining_count) FROM stdin;
\.


--
-- Data for Name: client_node_registrations; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.client_node_registrations (client_id, value, name) FROM stdin;
\.


--
-- Data for Name: client_scope; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.client_scope (id, name, realm_id, description, protocol) FROM stdin;
02523afa-a48d-410f-806d-9424a06258a1	offline_access	master	OpenID Connect built-in scope: offline_access	openid-connect
c8ee013d-6f5f-4537-abfc-223d033cfdef	role_list	master	SAML role list	saml
f9fec2d6-19ad-4058-9899-783805a982e8	profile	master	OpenID Connect built-in scope: profile	openid-connect
655b6977-f29c-49f7-a4a8-8879c90c326f	email	master	OpenID Connect built-in scope: email	openid-connect
98d28782-fa3a-4379-99e8-4ae7e8a29e2d	address	master	OpenID Connect built-in scope: address	openid-connect
c299f45b-9e42-4d79-94f3-2d63c4ad2b6c	phone	master	OpenID Connect built-in scope: phone	openid-connect
4924be0a-fffa-4c2f-8975-dd98138ae7a5	roles	master	OpenID Connect scope for add user roles to the access token	openid-connect
e0f4afb4-705c-4de2-b536-025e1522e270	web-origins	master	OpenID Connect scope for add allowed web origins to the access token	openid-connect
9a266253-af14-4478-8a65-33070b82df1a	microprofile-jwt	master	Microprofile - JWT built-in scope	openid-connect
44e568c9-e763-4a8f-979f-4b12932fe0dc	offline_access	ModMappings	OpenID Connect built-in scope: offline_access	openid-connect
05ef0639-aae9-4a1f-ad43-b8f6d1eb0a50	role_list	ModMappings	SAML role list	saml
30af825f-1ec9-45af-a678-279a6458f78d	profile	ModMappings	OpenID Connect built-in scope: profile	openid-connect
61b57419-d020-4777-a159-e3b13122842e	email	ModMappings	OpenID Connect built-in scope: email	openid-connect
880bb5d8-829b-4e64-b07d-913dab25091a	address	ModMappings	OpenID Connect built-in scope: address	openid-connect
2d9c89c5-4764-48eb-9140-c34aceb2b1c7	phone	ModMappings	OpenID Connect built-in scope: phone	openid-connect
88f3f2e7-c4fd-4439-a3a3-8fab2c64bd03	roles	ModMappings	OpenID Connect scope for add user roles to the access token	openid-connect
e682941a-b6e0-4a6a-8539-150b36cb85e9	web-origins	ModMappings	OpenID Connect scope for add allowed web origins to the access token	openid-connect
b9cbe9c2-5da1-4be4-a1e5-c308e6b222b3	microprofile-jwt	ModMappings	Microprofile - JWT built-in scope	openid-connect
\.


--
-- Data for Name: client_scope_attributes; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.client_scope_attributes (scope_id, value, name) FROM stdin;
02523afa-a48d-410f-806d-9424a06258a1	true	display.on.consent.screen
02523afa-a48d-410f-806d-9424a06258a1	${offlineAccessScopeConsentText}	consent.screen.text
c8ee013d-6f5f-4537-abfc-223d033cfdef	true	display.on.consent.screen
c8ee013d-6f5f-4537-abfc-223d033cfdef	${samlRoleListScopeConsentText}	consent.screen.text
f9fec2d6-19ad-4058-9899-783805a982e8	true	display.on.consent.screen
f9fec2d6-19ad-4058-9899-783805a982e8	${profileScopeConsentText}	consent.screen.text
f9fec2d6-19ad-4058-9899-783805a982e8	true	include.in.token.scope
655b6977-f29c-49f7-a4a8-8879c90c326f	true	display.on.consent.screen
655b6977-f29c-49f7-a4a8-8879c90c326f	${emailScopeConsentText}	consent.screen.text
655b6977-f29c-49f7-a4a8-8879c90c326f	true	include.in.token.scope
98d28782-fa3a-4379-99e8-4ae7e8a29e2d	true	display.on.consent.screen
98d28782-fa3a-4379-99e8-4ae7e8a29e2d	${addressScopeConsentText}	consent.screen.text
98d28782-fa3a-4379-99e8-4ae7e8a29e2d	true	include.in.token.scope
c299f45b-9e42-4d79-94f3-2d63c4ad2b6c	true	display.on.consent.screen
c299f45b-9e42-4d79-94f3-2d63c4ad2b6c	${phoneScopeConsentText}	consent.screen.text
c299f45b-9e42-4d79-94f3-2d63c4ad2b6c	true	include.in.token.scope
4924be0a-fffa-4c2f-8975-dd98138ae7a5	true	display.on.consent.screen
4924be0a-fffa-4c2f-8975-dd98138ae7a5	${rolesScopeConsentText}	consent.screen.text
4924be0a-fffa-4c2f-8975-dd98138ae7a5	false	include.in.token.scope
e0f4afb4-705c-4de2-b536-025e1522e270	false	display.on.consent.screen
e0f4afb4-705c-4de2-b536-025e1522e270		consent.screen.text
e0f4afb4-705c-4de2-b536-025e1522e270	false	include.in.token.scope
9a266253-af14-4478-8a65-33070b82df1a	false	display.on.consent.screen
9a266253-af14-4478-8a65-33070b82df1a	true	include.in.token.scope
44e568c9-e763-4a8f-979f-4b12932fe0dc	true	display.on.consent.screen
44e568c9-e763-4a8f-979f-4b12932fe0dc	${offlineAccessScopeConsentText}	consent.screen.text
05ef0639-aae9-4a1f-ad43-b8f6d1eb0a50	true	display.on.consent.screen
05ef0639-aae9-4a1f-ad43-b8f6d1eb0a50	${samlRoleListScopeConsentText}	consent.screen.text
30af825f-1ec9-45af-a678-279a6458f78d	true	display.on.consent.screen
30af825f-1ec9-45af-a678-279a6458f78d	${profileScopeConsentText}	consent.screen.text
30af825f-1ec9-45af-a678-279a6458f78d	true	include.in.token.scope
61b57419-d020-4777-a159-e3b13122842e	true	display.on.consent.screen
61b57419-d020-4777-a159-e3b13122842e	${emailScopeConsentText}	consent.screen.text
61b57419-d020-4777-a159-e3b13122842e	true	include.in.token.scope
880bb5d8-829b-4e64-b07d-913dab25091a	true	display.on.consent.screen
880bb5d8-829b-4e64-b07d-913dab25091a	${addressScopeConsentText}	consent.screen.text
880bb5d8-829b-4e64-b07d-913dab25091a	true	include.in.token.scope
2d9c89c5-4764-48eb-9140-c34aceb2b1c7	true	display.on.consent.screen
2d9c89c5-4764-48eb-9140-c34aceb2b1c7	${phoneScopeConsentText}	consent.screen.text
2d9c89c5-4764-48eb-9140-c34aceb2b1c7	true	include.in.token.scope
88f3f2e7-c4fd-4439-a3a3-8fab2c64bd03	true	display.on.consent.screen
88f3f2e7-c4fd-4439-a3a3-8fab2c64bd03	${rolesScopeConsentText}	consent.screen.text
88f3f2e7-c4fd-4439-a3a3-8fab2c64bd03	false	include.in.token.scope
e682941a-b6e0-4a6a-8539-150b36cb85e9	false	display.on.consent.screen
e682941a-b6e0-4a6a-8539-150b36cb85e9		consent.screen.text
e682941a-b6e0-4a6a-8539-150b36cb85e9	false	include.in.token.scope
b9cbe9c2-5da1-4be4-a1e5-c308e6b222b3	false	display.on.consent.screen
b9cbe9c2-5da1-4be4-a1e5-c308e6b222b3	true	include.in.token.scope
\.


--
-- Data for Name: client_scope_client; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.client_scope_client (client_id, scope_id, default_scope) FROM stdin;
c5f82688-a1d0-4d07-96f3-6dd303a5bd61	c8ee013d-6f5f-4537-abfc-223d033cfdef	t
bd3f6db1-5cd4-4be3-a107-367aa2e903dc	c8ee013d-6f5f-4537-abfc-223d033cfdef	t
e9c4b2a4-e48b-477a-b5f1-3e911fce718d	c8ee013d-6f5f-4537-abfc-223d033cfdef	t
6e4abf2f-cad7-4035-8014-f7b86990dee7	c8ee013d-6f5f-4537-abfc-223d033cfdef	t
24463fff-1058-4fc9-8c1e-9ecbcf7fa2e9	c8ee013d-6f5f-4537-abfc-223d033cfdef	t
c5f82688-a1d0-4d07-96f3-6dd303a5bd61	f9fec2d6-19ad-4058-9899-783805a982e8	t
c5f82688-a1d0-4d07-96f3-6dd303a5bd61	655b6977-f29c-49f7-a4a8-8879c90c326f	t
c5f82688-a1d0-4d07-96f3-6dd303a5bd61	4924be0a-fffa-4c2f-8975-dd98138ae7a5	t
c5f82688-a1d0-4d07-96f3-6dd303a5bd61	e0f4afb4-705c-4de2-b536-025e1522e270	t
c5f82688-a1d0-4d07-96f3-6dd303a5bd61	02523afa-a48d-410f-806d-9424a06258a1	f
c5f82688-a1d0-4d07-96f3-6dd303a5bd61	98d28782-fa3a-4379-99e8-4ae7e8a29e2d	f
c5f82688-a1d0-4d07-96f3-6dd303a5bd61	c299f45b-9e42-4d79-94f3-2d63c4ad2b6c	f
c5f82688-a1d0-4d07-96f3-6dd303a5bd61	9a266253-af14-4478-8a65-33070b82df1a	f
bd3f6db1-5cd4-4be3-a107-367aa2e903dc	f9fec2d6-19ad-4058-9899-783805a982e8	t
bd3f6db1-5cd4-4be3-a107-367aa2e903dc	655b6977-f29c-49f7-a4a8-8879c90c326f	t
bd3f6db1-5cd4-4be3-a107-367aa2e903dc	4924be0a-fffa-4c2f-8975-dd98138ae7a5	t
bd3f6db1-5cd4-4be3-a107-367aa2e903dc	e0f4afb4-705c-4de2-b536-025e1522e270	t
bd3f6db1-5cd4-4be3-a107-367aa2e903dc	02523afa-a48d-410f-806d-9424a06258a1	f
bd3f6db1-5cd4-4be3-a107-367aa2e903dc	98d28782-fa3a-4379-99e8-4ae7e8a29e2d	f
bd3f6db1-5cd4-4be3-a107-367aa2e903dc	c299f45b-9e42-4d79-94f3-2d63c4ad2b6c	f
bd3f6db1-5cd4-4be3-a107-367aa2e903dc	9a266253-af14-4478-8a65-33070b82df1a	f
e9c4b2a4-e48b-477a-b5f1-3e911fce718d	f9fec2d6-19ad-4058-9899-783805a982e8	t
e9c4b2a4-e48b-477a-b5f1-3e911fce718d	655b6977-f29c-49f7-a4a8-8879c90c326f	t
e9c4b2a4-e48b-477a-b5f1-3e911fce718d	4924be0a-fffa-4c2f-8975-dd98138ae7a5	t
e9c4b2a4-e48b-477a-b5f1-3e911fce718d	e0f4afb4-705c-4de2-b536-025e1522e270	t
e9c4b2a4-e48b-477a-b5f1-3e911fce718d	02523afa-a48d-410f-806d-9424a06258a1	f
e9c4b2a4-e48b-477a-b5f1-3e911fce718d	98d28782-fa3a-4379-99e8-4ae7e8a29e2d	f
e9c4b2a4-e48b-477a-b5f1-3e911fce718d	c299f45b-9e42-4d79-94f3-2d63c4ad2b6c	f
e9c4b2a4-e48b-477a-b5f1-3e911fce718d	9a266253-af14-4478-8a65-33070b82df1a	f
6e4abf2f-cad7-4035-8014-f7b86990dee7	f9fec2d6-19ad-4058-9899-783805a982e8	t
6e4abf2f-cad7-4035-8014-f7b86990dee7	655b6977-f29c-49f7-a4a8-8879c90c326f	t
6e4abf2f-cad7-4035-8014-f7b86990dee7	4924be0a-fffa-4c2f-8975-dd98138ae7a5	t
6e4abf2f-cad7-4035-8014-f7b86990dee7	e0f4afb4-705c-4de2-b536-025e1522e270	t
6e4abf2f-cad7-4035-8014-f7b86990dee7	02523afa-a48d-410f-806d-9424a06258a1	f
6e4abf2f-cad7-4035-8014-f7b86990dee7	98d28782-fa3a-4379-99e8-4ae7e8a29e2d	f
6e4abf2f-cad7-4035-8014-f7b86990dee7	c299f45b-9e42-4d79-94f3-2d63c4ad2b6c	f
6e4abf2f-cad7-4035-8014-f7b86990dee7	9a266253-af14-4478-8a65-33070b82df1a	f
24463fff-1058-4fc9-8c1e-9ecbcf7fa2e9	f9fec2d6-19ad-4058-9899-783805a982e8	t
24463fff-1058-4fc9-8c1e-9ecbcf7fa2e9	655b6977-f29c-49f7-a4a8-8879c90c326f	t
24463fff-1058-4fc9-8c1e-9ecbcf7fa2e9	4924be0a-fffa-4c2f-8975-dd98138ae7a5	t
24463fff-1058-4fc9-8c1e-9ecbcf7fa2e9	e0f4afb4-705c-4de2-b536-025e1522e270	t
24463fff-1058-4fc9-8c1e-9ecbcf7fa2e9	02523afa-a48d-410f-806d-9424a06258a1	f
24463fff-1058-4fc9-8c1e-9ecbcf7fa2e9	98d28782-fa3a-4379-99e8-4ae7e8a29e2d	f
24463fff-1058-4fc9-8c1e-9ecbcf7fa2e9	c299f45b-9e42-4d79-94f3-2d63c4ad2b6c	f
24463fff-1058-4fc9-8c1e-9ecbcf7fa2e9	9a266253-af14-4478-8a65-33070b82df1a	f
b1781787-3740-49f7-9015-51485579c1e5	c8ee013d-6f5f-4537-abfc-223d033cfdef	t
b1781787-3740-49f7-9015-51485579c1e5	f9fec2d6-19ad-4058-9899-783805a982e8	t
b1781787-3740-49f7-9015-51485579c1e5	655b6977-f29c-49f7-a4a8-8879c90c326f	t
b1781787-3740-49f7-9015-51485579c1e5	4924be0a-fffa-4c2f-8975-dd98138ae7a5	t
b1781787-3740-49f7-9015-51485579c1e5	e0f4afb4-705c-4de2-b536-025e1522e270	t
b1781787-3740-49f7-9015-51485579c1e5	02523afa-a48d-410f-806d-9424a06258a1	f
b1781787-3740-49f7-9015-51485579c1e5	98d28782-fa3a-4379-99e8-4ae7e8a29e2d	f
b1781787-3740-49f7-9015-51485579c1e5	c299f45b-9e42-4d79-94f3-2d63c4ad2b6c	f
b1781787-3740-49f7-9015-51485579c1e5	9a266253-af14-4478-8a65-33070b82df1a	f
b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	05ef0639-aae9-4a1f-ad43-b8f6d1eb0a50	t
e9f267a6-0ae2-4756-b8f3-7bf926e1db94	05ef0639-aae9-4a1f-ad43-b8f6d1eb0a50	t
f9564792-f154-4177-ac4c-0d47d2f4c026	05ef0639-aae9-4a1f-ad43-b8f6d1eb0a50	t
d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	05ef0639-aae9-4a1f-ad43-b8f6d1eb0a50	t
d8945854-74b3-460b-9089-00fbc8c14672	05ef0639-aae9-4a1f-ad43-b8f6d1eb0a50	t
b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	30af825f-1ec9-45af-a678-279a6458f78d	t
b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	61b57419-d020-4777-a159-e3b13122842e	t
b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	88f3f2e7-c4fd-4439-a3a3-8fab2c64bd03	t
b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	e682941a-b6e0-4a6a-8539-150b36cb85e9	t
b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	44e568c9-e763-4a8f-979f-4b12932fe0dc	f
b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	880bb5d8-829b-4e64-b07d-913dab25091a	f
b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	2d9c89c5-4764-48eb-9140-c34aceb2b1c7	f
b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	b9cbe9c2-5da1-4be4-a1e5-c308e6b222b3	f
e9f267a6-0ae2-4756-b8f3-7bf926e1db94	30af825f-1ec9-45af-a678-279a6458f78d	t
e9f267a6-0ae2-4756-b8f3-7bf926e1db94	61b57419-d020-4777-a159-e3b13122842e	t
e9f267a6-0ae2-4756-b8f3-7bf926e1db94	88f3f2e7-c4fd-4439-a3a3-8fab2c64bd03	t
e9f267a6-0ae2-4756-b8f3-7bf926e1db94	e682941a-b6e0-4a6a-8539-150b36cb85e9	t
e9f267a6-0ae2-4756-b8f3-7bf926e1db94	44e568c9-e763-4a8f-979f-4b12932fe0dc	f
e9f267a6-0ae2-4756-b8f3-7bf926e1db94	880bb5d8-829b-4e64-b07d-913dab25091a	f
e9f267a6-0ae2-4756-b8f3-7bf926e1db94	2d9c89c5-4764-48eb-9140-c34aceb2b1c7	f
e9f267a6-0ae2-4756-b8f3-7bf926e1db94	b9cbe9c2-5da1-4be4-a1e5-c308e6b222b3	f
f9564792-f154-4177-ac4c-0d47d2f4c026	30af825f-1ec9-45af-a678-279a6458f78d	t
f9564792-f154-4177-ac4c-0d47d2f4c026	61b57419-d020-4777-a159-e3b13122842e	t
f9564792-f154-4177-ac4c-0d47d2f4c026	88f3f2e7-c4fd-4439-a3a3-8fab2c64bd03	t
f9564792-f154-4177-ac4c-0d47d2f4c026	e682941a-b6e0-4a6a-8539-150b36cb85e9	t
f9564792-f154-4177-ac4c-0d47d2f4c026	44e568c9-e763-4a8f-979f-4b12932fe0dc	f
f9564792-f154-4177-ac4c-0d47d2f4c026	880bb5d8-829b-4e64-b07d-913dab25091a	f
f9564792-f154-4177-ac4c-0d47d2f4c026	2d9c89c5-4764-48eb-9140-c34aceb2b1c7	f
f9564792-f154-4177-ac4c-0d47d2f4c026	b9cbe9c2-5da1-4be4-a1e5-c308e6b222b3	f
d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	30af825f-1ec9-45af-a678-279a6458f78d	t
d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	61b57419-d020-4777-a159-e3b13122842e	t
d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	88f3f2e7-c4fd-4439-a3a3-8fab2c64bd03	t
d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	e682941a-b6e0-4a6a-8539-150b36cb85e9	t
d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	44e568c9-e763-4a8f-979f-4b12932fe0dc	f
d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	880bb5d8-829b-4e64-b07d-913dab25091a	f
d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	2d9c89c5-4764-48eb-9140-c34aceb2b1c7	f
d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	b9cbe9c2-5da1-4be4-a1e5-c308e6b222b3	f
d8945854-74b3-460b-9089-00fbc8c14672	30af825f-1ec9-45af-a678-279a6458f78d	t
d8945854-74b3-460b-9089-00fbc8c14672	61b57419-d020-4777-a159-e3b13122842e	t
d8945854-74b3-460b-9089-00fbc8c14672	88f3f2e7-c4fd-4439-a3a3-8fab2c64bd03	t
d8945854-74b3-460b-9089-00fbc8c14672	e682941a-b6e0-4a6a-8539-150b36cb85e9	t
d8945854-74b3-460b-9089-00fbc8c14672	44e568c9-e763-4a8f-979f-4b12932fe0dc	f
d8945854-74b3-460b-9089-00fbc8c14672	880bb5d8-829b-4e64-b07d-913dab25091a	f
d8945854-74b3-460b-9089-00fbc8c14672	2d9c89c5-4764-48eb-9140-c34aceb2b1c7	f
d8945854-74b3-460b-9089-00fbc8c14672	b9cbe9c2-5da1-4be4-a1e5-c308e6b222b3	f
aed69ab6-ca48-4b7f-a94d-629361b04bb8	05ef0639-aae9-4a1f-ad43-b8f6d1eb0a50	t
aed69ab6-ca48-4b7f-a94d-629361b04bb8	30af825f-1ec9-45af-a678-279a6458f78d	t
aed69ab6-ca48-4b7f-a94d-629361b04bb8	61b57419-d020-4777-a159-e3b13122842e	t
aed69ab6-ca48-4b7f-a94d-629361b04bb8	88f3f2e7-c4fd-4439-a3a3-8fab2c64bd03	t
aed69ab6-ca48-4b7f-a94d-629361b04bb8	e682941a-b6e0-4a6a-8539-150b36cb85e9	t
aed69ab6-ca48-4b7f-a94d-629361b04bb8	44e568c9-e763-4a8f-979f-4b12932fe0dc	f
aed69ab6-ca48-4b7f-a94d-629361b04bb8	880bb5d8-829b-4e64-b07d-913dab25091a	f
aed69ab6-ca48-4b7f-a94d-629361b04bb8	2d9c89c5-4764-48eb-9140-c34aceb2b1c7	f
aed69ab6-ca48-4b7f-a94d-629361b04bb8	b9cbe9c2-5da1-4be4-a1e5-c308e6b222b3	f
\.


--
-- Data for Name: client_scope_role_mapping; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.client_scope_role_mapping (scope_id, role_id) FROM stdin;
02523afa-a48d-410f-806d-9424a06258a1	5726c14b-12e1-4acf-ab64-a55d60b8c582
44e568c9-e763-4a8f-979f-4b12932fe0dc	770e4d48-d5a7-4795-8818-af03c8cf2d14
\.


--
-- Data for Name: client_session; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.client_session (id, client_id, redirect_uri, state, "timestamp", session_id, auth_method, realm_id, auth_user_id, current_action) FROM stdin;
\.


--
-- Data for Name: client_session_auth_status; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.client_session_auth_status (authenticator, status, client_session) FROM stdin;
\.


--
-- Data for Name: client_session_note; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.client_session_note (name, value, client_session) FROM stdin;
\.


--
-- Data for Name: client_session_prot_mapper; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.client_session_prot_mapper (protocol_mapper_id, client_session) FROM stdin;
\.


--
-- Data for Name: client_session_role; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.client_session_role (role_id, client_session) FROM stdin;
\.


--
-- Data for Name: client_user_session_note; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.client_user_session_note (name, value, client_session) FROM stdin;
\.


--
-- Data for Name: component; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.component (id, name, parent_id, provider_id, provider_type, realm_id, sub_type) FROM stdin;
a5a3513f-474d-411d-89ec-26a5bee1bff2	Trusted Hosts	master	trusted-hosts	org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy	master	anonymous
21686eaf-2171-4fc1-b705-316a6de491e3	Consent Required	master	consent-required	org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy	master	anonymous
aae4e606-5645-4753-8f86-d0714f4f300d	Full Scope Disabled	master	scope	org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy	master	anonymous
f4295ade-c984-48e0-8f72-cae2225e4de2	Max Clients Limit	master	max-clients	org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy	master	anonymous
c9a0a3c4-0029-463c-9cf9-877956a4edea	Allowed Protocol Mapper Types	master	allowed-protocol-mappers	org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy	master	anonymous
b3a82265-bea2-4862-9556-581675bec4c8	Allowed Client Scopes	master	allowed-client-templates	org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy	master	anonymous
7662d0f7-f530-4ed8-873d-757da22bcbba	Allowed Protocol Mapper Types	master	allowed-protocol-mappers	org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy	master	authenticated
48dd4bb2-0cce-48b0-b4e7-66f64a39c968	Allowed Client Scopes	master	allowed-client-templates	org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy	master	authenticated
944859e6-12b3-4c1b-a272-2d9db6c040ed	rsa-generated	master	rsa-generated	org.keycloak.keys.KeyProvider	master	\N
2c91020b-8eac-4975-a77b-61d8ca248b9c	hmac-generated	master	hmac-generated	org.keycloak.keys.KeyProvider	master	\N
55f3dfa0-8bd7-4f04-aadf-fb2e2f03f8e9	aes-generated	master	aes-generated	org.keycloak.keys.KeyProvider	master	\N
a4b9f34c-e62b-47f0-ae94-aa5862eb249e	rsa-generated	ModMappings	rsa-generated	org.keycloak.keys.KeyProvider	ModMappings	\N
b8ff6e57-592b-4c28-ac9f-b4c33290c49f	hmac-generated	ModMappings	hmac-generated	org.keycloak.keys.KeyProvider	ModMappings	\N
ffa4be73-928b-4ac9-a3ae-dda47a76dfa7	aes-generated	ModMappings	aes-generated	org.keycloak.keys.KeyProvider	ModMappings	\N
63ccd675-c04a-4a49-a795-6ff74acd3345	Trusted Hosts	ModMappings	trusted-hosts	org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy	ModMappings	anonymous
801c0329-51a5-4dca-81fa-67c78122ccc0	Consent Required	ModMappings	consent-required	org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy	ModMappings	anonymous
b8c99dcc-a87e-46f2-8f04-aff194fc1ac9	Full Scope Disabled	ModMappings	scope	org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy	ModMappings	anonymous
66cbb3b3-94b6-4bbf-b61d-afd2df8d09e0	Max Clients Limit	ModMappings	max-clients	org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy	ModMappings	anonymous
5cf763e3-52fe-4948-ac46-5a73845d4199	Allowed Protocol Mapper Types	ModMappings	allowed-protocol-mappers	org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy	ModMappings	anonymous
57ad63cf-f3b1-43bd-a0c3-8fa3bcbd74da	Allowed Client Scopes	ModMappings	allowed-client-templates	org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy	ModMappings	anonymous
db777b38-2a6f-43be-82bc-1ae153c195ba	Allowed Protocol Mapper Types	ModMappings	allowed-protocol-mappers	org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy	ModMappings	authenticated
4e6821f7-350e-43b5-8998-4c95c81bfd8e	Allowed Client Scopes	ModMappings	allowed-client-templates	org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy	ModMappings	authenticated
\.


--
-- Data for Name: component_config; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.component_config (id, component_id, name, value) FROM stdin;
8d72bc31-0a18-4ad0-9733-b400a50df94a	a5a3513f-474d-411d-89ec-26a5bee1bff2	client-uris-must-match	true
b226b1c5-ab3e-4d45-b444-ee6512bc53c1	a5a3513f-474d-411d-89ec-26a5bee1bff2	host-sending-registration-request-must-match	true
b746b336-b0fa-41cd-b3f3-8a6eef2a1dce	c9a0a3c4-0029-463c-9cf9-877956a4edea	allowed-protocol-mapper-types	saml-user-attribute-mapper
c29190cd-76b7-4b97-82bf-d17cd357c24a	c9a0a3c4-0029-463c-9cf9-877956a4edea	allowed-protocol-mapper-types	oidc-usermodel-property-mapper
78afa75e-50d6-4df1-b4b2-6d8eb62d5a8d	c9a0a3c4-0029-463c-9cf9-877956a4edea	allowed-protocol-mapper-types	oidc-address-mapper
9296b5db-e8df-4f20-9958-12a6353bb599	c9a0a3c4-0029-463c-9cf9-877956a4edea	allowed-protocol-mapper-types	oidc-sha256-pairwise-sub-mapper
daeae744-9f03-451c-b98b-6579a09c6d94	c9a0a3c4-0029-463c-9cf9-877956a4edea	allowed-protocol-mapper-types	saml-user-property-mapper
e6da434c-0d7c-46b1-b865-d5852bf8e9fc	c9a0a3c4-0029-463c-9cf9-877956a4edea	allowed-protocol-mapper-types	oidc-full-name-mapper
5047aa5c-a2fe-403e-8250-81463a216076	c9a0a3c4-0029-463c-9cf9-877956a4edea	allowed-protocol-mapper-types	oidc-usermodel-attribute-mapper
3120fd42-8fc5-44aa-899f-63fddc89526c	c9a0a3c4-0029-463c-9cf9-877956a4edea	allowed-protocol-mapper-types	saml-role-list-mapper
1373f318-b7a2-4ad2-953b-c7a19cf75fed	7662d0f7-f530-4ed8-873d-757da22bcbba	allowed-protocol-mapper-types	saml-user-property-mapper
3c1a63f5-9b84-4c93-a1f0-5aa39d7b92a8	7662d0f7-f530-4ed8-873d-757da22bcbba	allowed-protocol-mapper-types	oidc-usermodel-attribute-mapper
d87c24f2-d42b-4dd5-b1b3-45c048287ae4	7662d0f7-f530-4ed8-873d-757da22bcbba	allowed-protocol-mapper-types	oidc-full-name-mapper
0a2772fa-d98e-4f36-ad3e-5e0dc15f50cb	7662d0f7-f530-4ed8-873d-757da22bcbba	allowed-protocol-mapper-types	saml-role-list-mapper
07c3770b-a81d-4c09-8800-4cafba2657fd	7662d0f7-f530-4ed8-873d-757da22bcbba	allowed-protocol-mapper-types	oidc-usermodel-property-mapper
5f8311f8-191b-47d8-993f-030917a0d99a	7662d0f7-f530-4ed8-873d-757da22bcbba	allowed-protocol-mapper-types	oidc-address-mapper
1c43010b-26cf-4c1f-886e-95a9c38a3d34	7662d0f7-f530-4ed8-873d-757da22bcbba	allowed-protocol-mapper-types	saml-user-attribute-mapper
0b252d04-d568-4068-966a-9c80f55c6d94	7662d0f7-f530-4ed8-873d-757da22bcbba	allowed-protocol-mapper-types	oidc-sha256-pairwise-sub-mapper
5016bdf6-707a-4503-948f-630d692af183	f4295ade-c984-48e0-8f72-cae2225e4de2	max-clients	200
567b2e9d-dcd1-4132-997c-b9c7ec318ac5	b3a82265-bea2-4862-9556-581675bec4c8	allow-default-scopes	true
d9634805-e282-4833-8478-39f02f20d89a	48dd4bb2-0cce-48b0-b4e7-66f64a39c968	allow-default-scopes	true
6c0df741-74e7-4f45-adaf-b21a8370116d	2c91020b-8eac-4975-a77b-61d8ca248b9c	priority	100
654bb389-d523-44d3-85e3-4d8fba3f44f1	2c91020b-8eac-4975-a77b-61d8ca248b9c	secret	LxW2dEKlakgrUnJ2qelO3EQKwg26XYaEhIZdSkZbLuU9lVJW9IUO1dG3zFP-aHe5Ez3HibUwyC9BZ7Dy-M1gdg
57e4b9b9-aa67-4749-9546-353b900a2075	2c91020b-8eac-4975-a77b-61d8ca248b9c	kid	68aaeff8-35f9-42f9-bb05-84fc13e7919d
cb21474e-c9fb-4f11-9fb6-33a797d93c60	2c91020b-8eac-4975-a77b-61d8ca248b9c	algorithm	HS256
5d052d72-c2f0-4049-ae99-29f23f7d5ea7	944859e6-12b3-4c1b-a272-2d9db6c040ed	certificate	MIICmzCCAYMCBgFvr/9t7jANBgkqhkiG9w0BAQsFADARMQ8wDQYDVQQDDAZtYXN0ZXIwHhcNMjAwMTE2MjAxNDEwWhcNMzAwMTE2MjAxNTUwWjARMQ8wDQYDVQQDDAZtYXN0ZXIwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCNoDnINBhQ/oSe9okerJE4H//kWnmtvEaXjpbKdoL1ZvpYFebqbIWG5ttCCI/69azknIhrJEkbvvn9dK3g+atH9cZcskWS27LE8L97Y9CjD+hr5pJUepZnzUrSI3GZ3CgS9Zm9avWGezE0nF9+/6aeDy0niCdYex7IkR87cRlKR44qxqXd7iWkFxCLU0BwZ9m989Sxmnntxn0nuqFonwz5kL1s1492zH1U6Ewuf/lHcldKDrq6qwlYkCZsaXoe5rc4ZT02MRLRPlKSTItt2qVurg4elMlriYJ0MlayQROTR85k9e7/I1xnGcnr42nXQTZA8hkNVFs64F5YegqsvDJRAgMBAAEwDQYJKoZIhvcNAQELBQADggEBAH+FYDb59r42c4f+bq5riHNMuramG5VkxWyWKHhLXkMIy0o6XBmsnO3NCkLxGRvWy6ODskLfEENdBLQ/f7EXP6IVgSJTsx/sHedN6/IrHX+Zj1mCuPVnNpXRXnJNRCvkE3C/FxSnWMX23YJLjkF70bMGjdtbEItsZVkmcYeLj8QfrYlFJXzdL2RXYnb6d1r8giMqSsaHP3f+viQIEUK9t71EydBnWv07610ua/3rdMRWfUX7f7Y2PiseGMuE4xtl3k0v/kGjF4xXOw2s5v7ZrabCzD64c0r8QB2301M3j7XhwtbW9L7BwFbV43P3Braqf+n/QbaDiNVrOedmaYtWde4=
7a8c0c62-3338-4bc9-b614-6d3a783eaa99	944859e6-12b3-4c1b-a272-2d9db6c040ed	priority	100
19df9e39-e026-4e62-828d-f6f5cf03426b	944859e6-12b3-4c1b-a272-2d9db6c040ed	privateKey	MIIEogIBAAKCAQEAjaA5yDQYUP6EnvaJHqyROB//5Fp5rbxGl46WynaC9Wb6WBXm6myFhubbQgiP+vWs5JyIayRJG775/XSt4PmrR/XGXLJFktuyxPC/e2PQow/oa+aSVHqWZ81K0iNxmdwoEvWZvWr1hnsxNJxffv+mng8tJ4gnWHseyJEfO3EZSkeOKsal3e4lpBcQi1NAcGfZvfPUsZp57cZ9J7qhaJ8M+ZC9bNePdsx9VOhMLn/5R3JXSg66uqsJWJAmbGl6Hua3OGU9NjES0T5SkkyLbdqlbq4OHpTJa4mCdDJWskETk0fOZPXu/yNcZxnJ6+Np10E2QPIZDVRbOuBeWHoKrLwyUQIDAQABAoIBAHuHBx8SIStz86TbD9pLVhaIAp/gMkVQ9Jl4axmIqMz21uBBqjlKEmUJsrAz27Fe20BDL9GTwpiKyG1Dee3ClpybSDrN21Uufwy7l6g99Vpko6qJX5SDn/BF5T7IX2t91Q5EXaYTRrfXd6GVys1wmpk+T3otjXPlX4I84ersyMw5tG6CEPRjm1lK0eA3rtjF38tUA45LVfxPCiTMiCCM9ghSw80uAmGaNMQwvrt+Mvw69CHks8/kkl9ixx7zgvutYRteo8dKKD5uTGmpd56MhzRQUbiIfweYx8aOU5VzSiyRRmacF5mSofej3pALaV4P3CDdcTNV2eVTuok3NRk+IAECgYEAyz4gena+gYZFGoXqSLl/L7tzl8xqRisNplEx+oCvF2yjFhDOb3YJmtPxstR8LDuy8gdYtzmLiqQzsqOZCt/9fjKXJWIKkZeVf1RxfMk6H8uK1yDZwAWGLcuQ1AuLL7UxBCArnbBiHfFdJAzHV59LiV28W1EBG7VRIK7ZBQ128QECgYEAsmOKxcHK5yM/UhzDTZwtjMKBB/VAzqWLY625AbUMOZBvl6YxBX3zW8C9QtpR11XnJSSBFo2iXU3X13WJw52H+RxzESESyuKM243MJaK38hgCt/ohAVGgsvO3i0A9D9aMuPtyvDFpItOLVrgKEfqnCDNuRLtoKD8EEU/xlrk48VECgYBF5pwtAmILEi76LEyjroi71fCvqLJ0Z6JOLWbPDSsrF0YP9L/LPgGXqoVaSBJc9DtbWoN8oOIJUOgm7HVadCcvHB830DxqToQwQs61aFABV3PtAXNiw4OehIDPLRk100+EZ7wYg+169uRd3bdBv5uvJvn9PiLvcLDyWLBzOGpMAQKBgBYK2sDo20uIPXDGARP78lzsQ8lZ8rIWHo0okC2301ThlghlDrWhBIsX28sA8w/qm28pfZHt2lwxHDSRX35+XqjXbRh2v722FLWkZc9YteYCNYPKMn3ZOkQifVGFlIX6etT4rQq4CEe3YAZNI2FpWW5X/I4dohcnAjepay62oGShAoGAXNshBvffraKZzEepza8WNeAOAeW5/Bq7m2P+CgAC8hLiioARiOzGDRZBZ+hKC75EFgcOZEfVBUxAWer7m3RfZN2JI/3sY0SQglS87+IU2pisHT5qxmS2nbeaKAS4WWSzRZweZpMdTJSG2K6yRZ3y6ODNxPOVmLnfhVRcslK7aDc=
4debc80c-62a7-4e09-92c6-2c8668f11535	55f3dfa0-8bd7-4f04-aadf-fb2e2f03f8e9	priority	100
ddb36cc7-40ef-4ae3-9e51-464972ccc85e	55f3dfa0-8bd7-4f04-aadf-fb2e2f03f8e9	kid	fca66e83-1445-425a-9573-1f6369928743
cb3f6deb-46c5-48b2-bb10-5d3d2237bb5d	55f3dfa0-8bd7-4f04-aadf-fb2e2f03f8e9	secret	aIdj4x9jcgRogs_7jM7E6g
1dd899f9-d119-4196-bd4a-dce246ba90f3	b8ff6e57-592b-4c28-ac9f-b4c33290c49f	kid	a01a51de-067f-42da-94a6-a94d3ecac864
c07ec121-891b-460d-83f9-1ed8021bb4a1	b8ff6e57-592b-4c28-ac9f-b4c33290c49f	secret	qZcBythOhOa315gT-G5eJbvcK0IaM3bN5xsXHleshmd8YLeUm3cFvCPubzydbnpzhZv_rAYQGMYlPWtEO3R3WA
bb91a2e6-3888-4a54-b7e1-9c82fa973633	b8ff6e57-592b-4c28-ac9f-b4c33290c49f	priority	100
57a1b16e-0206-4121-9818-5f97d461b71f	b8ff6e57-592b-4c28-ac9f-b4c33290c49f	algorithm	HS256
fe443016-4824-46c6-a862-c9166769f573	a4b9f34c-e62b-47f0-ae94-aa5862eb249e	privateKey	MIIEpQIBAAKCAQEAqUl8sa+W09zsOi8B7pqtfSGCxgsKDQEexfT9/wPpYArXOT+pGPxhHZCF4/uGvaQEEC2ISo6VZ1LAN1u2Ekf+WHhig1wiKbJ1PQneKXRmQjnoX2fR1MMZyVTbEL0JAa5BDr4X9OpctOL1Ec+n+ksChSTJLxiJEE4ai39rtYsmmzyyw6bIOKbsYg3IY8jgh7bTzYKog3bNCH53b7vU7nt7Suj2kITsRES2EW4HXkhTSed+syAf3BXDT5Bvnwk/IoCFD2JgdMkrXMTimgTcCU1M8QWQqYeSK6WqPgCyLmZljWFX86S+YtDG8M2svY7rErzsQ9B1AWJpwxdfLVwxs924UQIDAQABAoIBAQCSYV32UyurhQOrCAy2on9TjQ+EafVF62sNp1ueEGbTSmxyL07rsQfUxaxQx6TqBJIS3xxTw9kCn/Zfp+jXA/O10lr01U0hGxPvNMIqvGDT8TiyjpZNUGrSwZIvdhX8ow3UTqFugNyq+PkKbrIEZqoGMRnZpHCYbcDcUND1CR7X6YpHk7WD4+uBeikYFVVeVa/f5rEHNwECykTAgxlgC/5WasV5DL2rhryIlnKJ18/keCj4kpqz0FrVt3HzFaawroMzUX+sdukKmWfDo7PFminlhkB0Z/eVZ8XZtAm2/gkTQx/TopW8v0JNUIRfDkC/v751qwMSqd881dQT63SIQiIBAoGBAOGWHDOgx8DJiSVI67wAGL56xBDn1K70MW7SG8mCPBlqBX1iNyvLJ6niifsMFP/DSGe2QZBUVdwixz8m+XcnlSZZnbjI0jgf5FxX3n+0BnvNtlMJpsyrG+tl1KXvkkphC+YB9Qm30MMF4tSFDEuaLck6iPJk/WTt/l9Ltcgv/HDxAoGBAMAcQxiBQojaOeEtovAMIekwgjUnoj4zCn0vbnGDJRo/bMuvH2dqJ4FXkO0gcGdOemtpSlUeG8kGbB9CdGnEWiJVvetiViBR28+jzBPNzFr4ui7DfLykRQLbbyX3u96iInuz8Jjt498NdWx1aW4QPoRuS/KQaPezk+pf/MsDJb1hAoGBAK0RA+vb8sQGGhCfxDMEg/dGqjsqEPJ0Z6RUz3qmTsNUooeSHS+c+X4NTbxrhYS+5kKjAePfv8tWZzQdC5CNrpCweh90+kqStRjfVNT2YW8D4FgOdIha0d2jzyPLC5nCoGVCpSJigmfMkTxoIomx7GSQYtZMAlc3e0rTA9BkeDCRAoGBAIdf4xATljUlwn+hTx5lDkhqvHaElVnLUerhT6yBw9V5OEtX/oM7VNdaQ3A12b+Sl8W1DoJokx/XfL2ScMJPcUycqxaB1zszM/hf7mFONmEswNZPP0kXYOIgi3Rv3F2IV1BgfnzlthvqOgLRwhejLsrXhoFJrg5O5z7ToSnV7/RBAoGAXds3i/JsQEwtlKjo4//Mjsst2Hq1K4Kn0RtceymtTol3bXgiHlD7Ez9IP035zgUdVX+dFxNHAjADELUmjV0cBo4dba2tbHhQn/LcC/DkitRn/fJZ8jwU948q/WgH5RLScXv5D8O1780C5JKVjw2zj8/NHaun4adXqvB7jpc+0Wc=
bce76e5c-76fc-43b9-b57b-ab63309f9e43	a4b9f34c-e62b-47f0-ae94-aa5862eb249e	certificate	MIICpTCCAY0CBgFvsALGPzANBgkqhkiG9w0BAQsFADAWMRQwEgYDVQQDDAtNb2RNYXBwaW5nczAeFw0yMDAxMTYyMDE3NDlaFw0zMDAxMTYyMDE5MjlaMBYxFDASBgNVBAMMC01vZE1hcHBpbmdzMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqUl8sa+W09zsOi8B7pqtfSGCxgsKDQEexfT9/wPpYArXOT+pGPxhHZCF4/uGvaQEEC2ISo6VZ1LAN1u2Ekf+WHhig1wiKbJ1PQneKXRmQjnoX2fR1MMZyVTbEL0JAa5BDr4X9OpctOL1Ec+n+ksChSTJLxiJEE4ai39rtYsmmzyyw6bIOKbsYg3IY8jgh7bTzYKog3bNCH53b7vU7nt7Suj2kITsRES2EW4HXkhTSed+syAf3BXDT5Bvnwk/IoCFD2JgdMkrXMTimgTcCU1M8QWQqYeSK6WqPgCyLmZljWFX86S+YtDG8M2svY7rErzsQ9B1AWJpwxdfLVwxs924UQIDAQABMA0GCSqGSIb3DQEBCwUAA4IBAQChage3TM5glzj8/EeM0iZ5rjnrK1NKcx1ymVdvNDGyuNBqwSv6CFf+8IuFlA7XXTx20nYEUyEadHH2O3mF+scO9LESaF7IcNtMz9EnVEfNCmJsWDbXfV4bQ2PpZKCHhh0/uLRdOJ25s8VDDLLqw80VbQBSOU7+W96ket739xDu96M1oISdkMjMCQlrF7Uh5SMr3uLN9xPPG0AFNlgqWv2BaVjnqLL4iUbb5+TS0RD8B0AdXLH+7TPHqAt6ypxgAm+dorXiF5XBqSNwN7otqhaOen21rvwujvC86fZef+wxz5pCwFMIg+/qipGkiYjTXI6qHDtjrDI4++YVV/j7aUJO
a2385159-8178-4b3b-8bfd-581c9a845d39	a4b9f34c-e62b-47f0-ae94-aa5862eb249e	priority	100
8208794e-42ab-459b-9765-a62c2b17ce75	ffa4be73-928b-4ac9-a3ae-dda47a76dfa7	priority	100
73984645-f54c-4eed-a206-9a52fda4d488	ffa4be73-928b-4ac9-a3ae-dda47a76dfa7	kid	cef03205-6fdf-41be-a1f9-2f4d70ce2624
3cf97da9-54f6-455c-af68-a03f76caedf5	ffa4be73-928b-4ac9-a3ae-dda47a76dfa7	secret	izG1D5cYOwHZsMuGRF4Eqw
1ba7ac95-0801-4417-b2b4-cf5aa8af0e49	66cbb3b3-94b6-4bbf-b61d-afd2df8d09e0	max-clients	200
2340d5a9-ea7b-4b4c-b4be-6b0c02e36c35	5cf763e3-52fe-4948-ac46-5a73845d4199	allowed-protocol-mapper-types	saml-role-list-mapper
e307865d-7a61-412e-8744-1866354e57ec	5cf763e3-52fe-4948-ac46-5a73845d4199	allowed-protocol-mapper-types	oidc-sha256-pairwise-sub-mapper
735c2fc4-b0ce-40b4-9359-c3e77b996941	5cf763e3-52fe-4948-ac46-5a73845d4199	allowed-protocol-mapper-types	saml-user-attribute-mapper
0f69dd86-07e3-4450-b3c2-8a0479051d32	5cf763e3-52fe-4948-ac46-5a73845d4199	allowed-protocol-mapper-types	oidc-full-name-mapper
af8a0e5a-a677-4c0a-a72c-2a11e2172012	5cf763e3-52fe-4948-ac46-5a73845d4199	allowed-protocol-mapper-types	oidc-usermodel-attribute-mapper
885216a9-e8b2-4323-871c-d80e66dc8a62	5cf763e3-52fe-4948-ac46-5a73845d4199	allowed-protocol-mapper-types	oidc-usermodel-property-mapper
62c1dca2-6cc7-42c9-8b0a-31a6878b83c8	5cf763e3-52fe-4948-ac46-5a73845d4199	allowed-protocol-mapper-types	saml-user-property-mapper
7fded3cd-07e5-4d64-ab90-5ed4ca41ff9f	5cf763e3-52fe-4948-ac46-5a73845d4199	allowed-protocol-mapper-types	oidc-address-mapper
7f27b1d9-6d94-4306-a94a-6a2484cea236	57ad63cf-f3b1-43bd-a0c3-8fa3bcbd74da	allow-default-scopes	true
506e7d2f-2c57-46f6-aacc-cf695def14c1	db777b38-2a6f-43be-82bc-1ae153c195ba	allowed-protocol-mapper-types	oidc-usermodel-attribute-mapper
75fe8113-6dc2-469a-83b2-42a67934d5cb	db777b38-2a6f-43be-82bc-1ae153c195ba	allowed-protocol-mapper-types	oidc-full-name-mapper
c28d5045-1395-4654-aa2a-d60f3f5b64e3	db777b38-2a6f-43be-82bc-1ae153c195ba	allowed-protocol-mapper-types	saml-user-attribute-mapper
567d8d77-89c5-48ba-b0d4-91b54f782732	db777b38-2a6f-43be-82bc-1ae153c195ba	allowed-protocol-mapper-types	oidc-usermodel-property-mapper
e3e425ca-a2a1-4080-ae3b-6b5b3ea36f44	db777b38-2a6f-43be-82bc-1ae153c195ba	allowed-protocol-mapper-types	saml-role-list-mapper
18d0bbab-aad7-4bac-a83c-3a77484753bf	db777b38-2a6f-43be-82bc-1ae153c195ba	allowed-protocol-mapper-types	oidc-address-mapper
d73be5a9-6840-4d19-8567-6411a43427b1	db777b38-2a6f-43be-82bc-1ae153c195ba	allowed-protocol-mapper-types	oidc-sha256-pairwise-sub-mapper
688b4ddb-66cd-4bfa-8970-70282b913089	db777b38-2a6f-43be-82bc-1ae153c195ba	allowed-protocol-mapper-types	saml-user-property-mapper
c6f4f425-0f74-4faa-acec-658cfb267095	4e6821f7-350e-43b5-8998-4c95c81bfd8e	allow-default-scopes	true
52f2d54a-d739-46ee-89eb-5c66e38d14bd	63ccd675-c04a-4a49-a795-6ff74acd3345	host-sending-registration-request-must-match	true
4c5345a7-a2fe-4bd1-a149-03af9a64e239	63ccd675-c04a-4a49-a795-6ff74acd3345	client-uris-must-match	true
\.


--
-- Data for Name: composite_role; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.composite_role (composite, child_role) FROM stdin;
b21192ad-92c5-4b28-a87b-361e66aab625	1f73a5d6-fd8c-4015-b578-8df644251959
b21192ad-92c5-4b28-a87b-361e66aab625	880bf334-f8bf-4735-877c-0f0eff7970b8
b21192ad-92c5-4b28-a87b-361e66aab625	61fde2d6-e74f-4cd5-b274-2f22c913db0a
b21192ad-92c5-4b28-a87b-361e66aab625	e9a7ed8d-81ea-4d12-a2a8-3bcae99817e7
b21192ad-92c5-4b28-a87b-361e66aab625	fff8f672-002a-4639-85ca-556bc14ea62d
b21192ad-92c5-4b28-a87b-361e66aab625	526a4da7-73de-4d15-8b95-8c36bd35c7b5
b21192ad-92c5-4b28-a87b-361e66aab625	8339383b-96f0-4a93-a227-03a798dca271
b21192ad-92c5-4b28-a87b-361e66aab625	360adbb1-fe02-43f2-bf7f-ea4d0aa9a45c
b21192ad-92c5-4b28-a87b-361e66aab625	33ce929f-7839-4c63-883e-3f9e1bbedf83
b21192ad-92c5-4b28-a87b-361e66aab625	d308da3e-e9c3-4b05-8a76-b0ef249eb2c1
b21192ad-92c5-4b28-a87b-361e66aab625	5d4aeb28-ce92-4612-8e38-54ccf2785abc
b21192ad-92c5-4b28-a87b-361e66aab625	d61445e7-b340-429c-bcc7-c71a7b463c1f
b21192ad-92c5-4b28-a87b-361e66aab625	4f2c31f3-89af-408d-9a72-ff8dd2e93b04
b21192ad-92c5-4b28-a87b-361e66aab625	6c627d35-f93c-4ca8-a7b9-1d2f728bf097
b21192ad-92c5-4b28-a87b-361e66aab625	2d183fc2-adf3-4a3f-86e7-2d03a37d93ac
b21192ad-92c5-4b28-a87b-361e66aab625	6cf50abb-2c1e-49f6-b5f2-c7f2afce2ce9
b21192ad-92c5-4b28-a87b-361e66aab625	60fbec97-898d-4d3b-9327-9281394efb2f
b21192ad-92c5-4b28-a87b-361e66aab625	d5d26a2f-9ad2-49d5-9667-18d38221bff6
e9a7ed8d-81ea-4d12-a2a8-3bcae99817e7	2d183fc2-adf3-4a3f-86e7-2d03a37d93ac
e9a7ed8d-81ea-4d12-a2a8-3bcae99817e7	d5d26a2f-9ad2-49d5-9667-18d38221bff6
fff8f672-002a-4639-85ca-556bc14ea62d	6cf50abb-2c1e-49f6-b5f2-c7f2afce2ce9
7ffebb0e-73d8-4f0b-acee-44b682519130	c340dd29-1fcc-4fe4-9b5d-f1d3ae28b350
b21192ad-92c5-4b28-a87b-361e66aab625	261fe951-b52c-4525-9b3b-6413cc75d1e8
b21192ad-92c5-4b28-a87b-361e66aab625	ad64e08c-fb68-40d7-9b6d-f90d334942ff
b21192ad-92c5-4b28-a87b-361e66aab625	3596a222-eb39-4f9d-9693-023334d1d771
b21192ad-92c5-4b28-a87b-361e66aab625	3fe24019-5824-4653-ba77-4e52bd81b704
b21192ad-92c5-4b28-a87b-361e66aab625	3dfcf3c3-ea9b-4f4c-a5cd-72b3d69581b4
b21192ad-92c5-4b28-a87b-361e66aab625	c486ee7e-d3e0-413c-88ef-3ebd8d9dadd8
b21192ad-92c5-4b28-a87b-361e66aab625	748064a7-2e42-4bff-acca-dad4308a1bb8
b21192ad-92c5-4b28-a87b-361e66aab625	d1320b12-3020-4e85-94a3-faff695a2659
b21192ad-92c5-4b28-a87b-361e66aab625	735c6239-5c3e-41a1-bb60-11e7046927e5
b21192ad-92c5-4b28-a87b-361e66aab625	8251c5fb-14ad-4f4b-ba69-2a6802dace79
b21192ad-92c5-4b28-a87b-361e66aab625	967b044d-2b0e-4d75-b3a8-675be1f522d0
b21192ad-92c5-4b28-a87b-361e66aab625	3fb9fe09-94c8-46d0-b517-c930df083a49
b21192ad-92c5-4b28-a87b-361e66aab625	3e80cdf9-9c5f-40bf-a3b7-7cb080d7c3ba
b21192ad-92c5-4b28-a87b-361e66aab625	48e84a51-25e0-44ce-931a-c8b6b78ca209
b21192ad-92c5-4b28-a87b-361e66aab625	2e189751-c8ca-4786-8ae1-7033a8b349fa
b21192ad-92c5-4b28-a87b-361e66aab625	b6c9db9b-812f-4023-9bc8-3169885ac47d
b21192ad-92c5-4b28-a87b-361e66aab625	386fd497-4cd6-4d38-9e36-40909dcd3052
b21192ad-92c5-4b28-a87b-361e66aab625	aaf59ef8-723f-4b4b-8ff3-b2895d9fe09d
3fe24019-5824-4653-ba77-4e52bd81b704	2e189751-c8ca-4786-8ae1-7033a8b349fa
3fe24019-5824-4653-ba77-4e52bd81b704	aaf59ef8-723f-4b4b-8ff3-b2895d9fe09d
3dfcf3c3-ea9b-4f4c-a5cd-72b3d69581b4	b6c9db9b-812f-4023-9bc8-3169885ac47d
b0bc8ff0-764b-4347-b08a-e227401183df	e4142a3a-5a3f-42a4-b311-91e4b997f223
b0bc8ff0-764b-4347-b08a-e227401183df	5291fb1a-46f5-4029-9a9e-9a23952110c4
b0bc8ff0-764b-4347-b08a-e227401183df	8e0a2aa0-41be-4fed-b9db-18eac27f9c78
b0bc8ff0-764b-4347-b08a-e227401183df	aa9c3144-7c50-4db6-b088-ebcb7206266f
b0bc8ff0-764b-4347-b08a-e227401183df	fc19eb56-00a7-416e-a95e-e44eea129a45
b0bc8ff0-764b-4347-b08a-e227401183df	ce080e12-e6a1-416f-a323-9e44000f1961
b0bc8ff0-764b-4347-b08a-e227401183df	6915a553-228e-440d-9772-e994f9084477
b0bc8ff0-764b-4347-b08a-e227401183df	ae720494-acd5-4c06-bb9c-a7381e2b815f
b0bc8ff0-764b-4347-b08a-e227401183df	4f41266f-9074-41e7-9e77-f26adf921217
b0bc8ff0-764b-4347-b08a-e227401183df	84c30bf0-db52-47b8-aeff-22738a3face6
b0bc8ff0-764b-4347-b08a-e227401183df	f3f2bf16-7162-4f89-bb51-1b5ef45aade2
b0bc8ff0-764b-4347-b08a-e227401183df	f7e5962d-1290-4939-97fd-6e20bcd7996c
b0bc8ff0-764b-4347-b08a-e227401183df	d77fe98d-a72e-4430-85d4-7cd18dcc5aa8
b0bc8ff0-764b-4347-b08a-e227401183df	df264a00-54ff-41e3-af59-74e84d162f6f
b0bc8ff0-764b-4347-b08a-e227401183df	d19fa9aa-0321-4c47-86db-2a4c3913bc29
b0bc8ff0-764b-4347-b08a-e227401183df	86dcf10c-f726-438e-83ed-9ccf16f83553
b0bc8ff0-764b-4347-b08a-e227401183df	7bd8cd76-e01a-49d4-b72f-ce0b6b2bcce3
8e0a2aa0-41be-4fed-b9db-18eac27f9c78	7bd8cd76-e01a-49d4-b72f-ce0b6b2bcce3
8e0a2aa0-41be-4fed-b9db-18eac27f9c78	df264a00-54ff-41e3-af59-74e84d162f6f
aa9c3144-7c50-4db6-b088-ebcb7206266f	d19fa9aa-0321-4c47-86db-2a4c3913bc29
b21192ad-92c5-4b28-a87b-361e66aab625	022eba55-ca1f-4037-9b7c-a0dc234e4484
625aa352-faf9-4bfe-99f1-5f270200eb75	6c425314-8bd4-417f-a624-685a3ea2fa07
b0bc8ff0-764b-4347-b08a-e227401183df	5b33f0b7-bc80-483f-a102-6be01ae64c30
\.


--
-- Data for Name: credential; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.credential (id, salt, type, user_id, created_date, user_label, secret_data, credential_data, priority) FROM stdin;
8df83a6a-fcb6-4dc8-bae6-105d20925bc8	\N	password	5c150731-d9e2-41c0-8091-836e67699cf1	1593975894615	\N	{"value":"Ct7MSo1BeZn11IKWxLQT4nKMAthltyPjM92SMYQufmFsxx5v6KmfliItAQdzXNJixZTB/wbUueiqWUG3s6pqug==","salt":"fBH0GyzLaRhWI/RNz8rFjA=="}	{"hashIterations":27500,"algorithm":"pbkdf2-sha256"}	10
1ecc8e6f-28c7-49d0-8093-44b08a6fbae8	\N	password	5cb3ca77-47ab-4721-a1ad-7b1868559aa5	1593981942967	\N	{"value":"H/XKBBxpw41bsLKJKQYEdYnSSBv2hhMdJHqx4dioMrLN+5dkcGPc2RSfFM3DM/+Gqi+BGyUft32gAS3waEY53A==","salt":"62H/gRZycdUN/LFQ1Xz2vw=="}	{"hashIterations":27500,"algorithm":"pbkdf2-sha256"}	10
\.


--
-- Data for Name: databasechangelog; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.databasechangelog (id, author, filename, dateexecuted, orderexecuted, exectype, md5sum, description, comments, tag, liquibase, contexts, labels, deployment_id) FROM stdin;
1.0.0.Final-KEYCLOAK-5461	sthorger@redhat.com	META-INF/jpa-changelog-1.0.0.Final.xml	2020-01-16 20:15:41.553372	1	EXECUTED	7:4e70412f24a3f382c82183742ec79317	createTable tableName=APPLICATION_DEFAULT_ROLES; createTable tableName=CLIENT; createTable tableName=CLIENT_SESSION; createTable tableName=CLIENT_SESSION_ROLE; createTable tableName=COMPOSITE_ROLE; createTable tableName=CREDENTIAL; createTable tab...		\N	3.5.4	\N	\N	9205741049
1.0.0.Final-KEYCLOAK-5461	sthorger@redhat.com	META-INF/db2-jpa-changelog-1.0.0.Final.xml	2020-01-16 20:15:41.573719	2	MARK_RAN	7:cb16724583e9675711801c6875114f28	createTable tableName=APPLICATION_DEFAULT_ROLES; createTable tableName=CLIENT; createTable tableName=CLIENT_SESSION; createTable tableName=CLIENT_SESSION_ROLE; createTable tableName=COMPOSITE_ROLE; createTable tableName=CREDENTIAL; createTable tab...		\N	3.5.4	\N	\N	9205741049
1.1.0.Beta1	sthorger@redhat.com	META-INF/jpa-changelog-1.1.0.Beta1.xml	2020-01-16 20:15:41.636833	3	EXECUTED	7:0310eb8ba07cec616460794d42ade0fa	delete tableName=CLIENT_SESSION_ROLE; delete tableName=CLIENT_SESSION; delete tableName=USER_SESSION; createTable tableName=CLIENT_ATTRIBUTES; createTable tableName=CLIENT_SESSION_NOTE; createTable tableName=APP_NODE_REGISTRATIONS; addColumn table...		\N	3.5.4	\N	\N	9205741049
1.1.0.Final	sthorger@redhat.com	META-INF/jpa-changelog-1.1.0.Final.xml	2020-01-16 20:15:41.64894	4	EXECUTED	7:5d25857e708c3233ef4439df1f93f012	renameColumn newColumnName=EVENT_TIME, oldColumnName=TIME, tableName=EVENT_ENTITY		\N	3.5.4	\N	\N	9205741049
1.2.0.Beta1	psilva@redhat.com	META-INF/jpa-changelog-1.2.0.Beta1.xml	2020-01-16 20:15:41.815397	5	EXECUTED	7:c7a54a1041d58eb3817a4a883b4d4e84	delete tableName=CLIENT_SESSION_ROLE; delete tableName=CLIENT_SESSION_NOTE; delete tableName=CLIENT_SESSION; delete tableName=USER_SESSION; createTable tableName=PROTOCOL_MAPPER; createTable tableName=PROTOCOL_MAPPER_CONFIG; createTable tableName=...		\N	3.5.4	\N	\N	9205741049
1.2.0.Beta1	psilva@redhat.com	META-INF/db2-jpa-changelog-1.2.0.Beta1.xml	2020-01-16 20:15:41.82259	6	MARK_RAN	7:2e01012df20974c1c2a605ef8afe25b7	delete tableName=CLIENT_SESSION_ROLE; delete tableName=CLIENT_SESSION_NOTE; delete tableName=CLIENT_SESSION; delete tableName=USER_SESSION; createTable tableName=PROTOCOL_MAPPER; createTable tableName=PROTOCOL_MAPPER_CONFIG; createTable tableName=...		\N	3.5.4	\N	\N	9205741049
1.2.0.RC1	bburke@redhat.com	META-INF/jpa-changelog-1.2.0.CR1.xml	2020-01-16 20:15:41.970493	7	EXECUTED	7:0f08df48468428e0f30ee59a8ec01a41	delete tableName=CLIENT_SESSION_ROLE; delete tableName=CLIENT_SESSION_NOTE; delete tableName=CLIENT_SESSION; delete tableName=USER_SESSION_NOTE; delete tableName=USER_SESSION; createTable tableName=MIGRATION_MODEL; createTable tableName=IDENTITY_P...		\N	3.5.4	\N	\N	9205741049
1.2.0.RC1	bburke@redhat.com	META-INF/db2-jpa-changelog-1.2.0.CR1.xml	2020-01-16 20:15:41.978311	8	MARK_RAN	7:a77ea2ad226b345e7d689d366f185c8c	delete tableName=CLIENT_SESSION_ROLE; delete tableName=CLIENT_SESSION_NOTE; delete tableName=CLIENT_SESSION; delete tableName=USER_SESSION_NOTE; delete tableName=USER_SESSION; createTable tableName=MIGRATION_MODEL; createTable tableName=IDENTITY_P...		\N	3.5.4	\N	\N	9205741049
1.2.0.Final	keycloak	META-INF/jpa-changelog-1.2.0.Final.xml	2020-01-16 20:15:41.994729	9	EXECUTED	7:a3377a2059aefbf3b90ebb4c4cc8e2ab	update tableName=CLIENT; update tableName=CLIENT; update tableName=CLIENT		\N	3.5.4	\N	\N	9205741049
1.3.0	bburke@redhat.com	META-INF/jpa-changelog-1.3.0.xml	2020-01-16 20:15:42.12113	10	EXECUTED	7:04c1dbedc2aa3e9756d1a1668e003451	delete tableName=CLIENT_SESSION_ROLE; delete tableName=CLIENT_SESSION_PROT_MAPPER; delete tableName=CLIENT_SESSION_NOTE; delete tableName=CLIENT_SESSION; delete tableName=USER_SESSION_NOTE; delete tableName=USER_SESSION; createTable tableName=ADMI...		\N	3.5.4	\N	\N	9205741049
1.4.0	bburke@redhat.com	META-INF/jpa-changelog-1.4.0.xml	2020-01-16 20:15:42.190695	11	EXECUTED	7:36ef39ed560ad07062d956db861042ba	delete tableName=CLIENT_SESSION_AUTH_STATUS; delete tableName=CLIENT_SESSION_ROLE; delete tableName=CLIENT_SESSION_PROT_MAPPER; delete tableName=CLIENT_SESSION_NOTE; delete tableName=CLIENT_SESSION; delete tableName=USER_SESSION_NOTE; delete table...		\N	3.5.4	\N	\N	9205741049
1.4.0	bburke@redhat.com	META-INF/db2-jpa-changelog-1.4.0.xml	2020-01-16 20:15:42.194666	12	MARK_RAN	7:d909180b2530479a716d3f9c9eaea3d7	delete tableName=CLIENT_SESSION_AUTH_STATUS; delete tableName=CLIENT_SESSION_ROLE; delete tableName=CLIENT_SESSION_PROT_MAPPER; delete tableName=CLIENT_SESSION_NOTE; delete tableName=CLIENT_SESSION; delete tableName=USER_SESSION_NOTE; delete table...		\N	3.5.4	\N	\N	9205741049
1.5.0	bburke@redhat.com	META-INF/jpa-changelog-1.5.0.xml	2020-01-16 20:15:42.21621	13	EXECUTED	7:cf12b04b79bea5152f165eb41f3955f6	delete tableName=CLIENT_SESSION_AUTH_STATUS; delete tableName=CLIENT_SESSION_ROLE; delete tableName=CLIENT_SESSION_PROT_MAPPER; delete tableName=CLIENT_SESSION_NOTE; delete tableName=CLIENT_SESSION; delete tableName=USER_SESSION_NOTE; delete table...		\N	3.5.4	\N	\N	9205741049
1.6.1_from15	mposolda@redhat.com	META-INF/jpa-changelog-1.6.1.xml	2020-01-16 20:15:42.247826	14	EXECUTED	7:7e32c8f05c755e8675764e7d5f514509	addColumn tableName=REALM; addColumn tableName=KEYCLOAK_ROLE; addColumn tableName=CLIENT; createTable tableName=OFFLINE_USER_SESSION; createTable tableName=OFFLINE_CLIENT_SESSION; addPrimaryKey constraintName=CONSTRAINT_OFFL_US_SES_PK2, tableName=...		\N	3.5.4	\N	\N	9205741049
1.6.1_from16-pre	mposolda@redhat.com	META-INF/jpa-changelog-1.6.1.xml	2020-01-16 20:15:42.251562	15	MARK_RAN	7:980ba23cc0ec39cab731ce903dd01291	delete tableName=OFFLINE_CLIENT_SESSION; delete tableName=OFFLINE_USER_SESSION		\N	3.5.4	\N	\N	9205741049
1.6.1_from16	mposolda@redhat.com	META-INF/jpa-changelog-1.6.1.xml	2020-01-16 20:15:42.255348	16	MARK_RAN	7:2fa220758991285312eb84f3b4ff5336	dropPrimaryKey constraintName=CONSTRAINT_OFFLINE_US_SES_PK, tableName=OFFLINE_USER_SESSION; dropPrimaryKey constraintName=CONSTRAINT_OFFLINE_CL_SES_PK, tableName=OFFLINE_CLIENT_SESSION; addColumn tableName=OFFLINE_USER_SESSION; update tableName=OF...		\N	3.5.4	\N	\N	9205741049
1.6.1	mposolda@redhat.com	META-INF/jpa-changelog-1.6.1.xml	2020-01-16 20:15:42.258924	17	EXECUTED	7:d41d8cd98f00b204e9800998ecf8427e	empty		\N	3.5.4	\N	\N	9205741049
1.7.0	bburke@redhat.com	META-INF/jpa-changelog-1.7.0.xml	2020-01-16 20:15:42.330436	18	EXECUTED	7:91ace540896df890cc00a0490ee52bbc	createTable tableName=KEYCLOAK_GROUP; createTable tableName=GROUP_ROLE_MAPPING; createTable tableName=GROUP_ATTRIBUTE; createTable tableName=USER_GROUP_MEMBERSHIP; createTable tableName=REALM_DEFAULT_GROUPS; addColumn tableName=IDENTITY_PROVIDER; ...		\N	3.5.4	\N	\N	9205741049
1.8.0	mposolda@redhat.com	META-INF/jpa-changelog-1.8.0.xml	2020-01-16 20:15:42.403467	19	EXECUTED	7:c31d1646dfa2618a9335c00e07f89f24	addColumn tableName=IDENTITY_PROVIDER; createTable tableName=CLIENT_TEMPLATE; createTable tableName=CLIENT_TEMPLATE_ATTRIBUTES; createTable tableName=TEMPLATE_SCOPE_MAPPING; dropNotNullConstraint columnName=CLIENT_ID, tableName=PROTOCOL_MAPPER; ad...		\N	3.5.4	\N	\N	9205741049
1.8.0-2	keycloak	META-INF/jpa-changelog-1.8.0.xml	2020-01-16 20:15:42.419274	20	EXECUTED	7:df8bc21027a4f7cbbb01f6344e89ce07	dropDefaultValue columnName=ALGORITHM, tableName=CREDENTIAL; update tableName=CREDENTIAL		\N	3.5.4	\N	\N	9205741049
authz-3.4.0.CR1-resource-server-pk-change-part1	glavoie@gmail.com	META-INF/jpa-changelog-authz-3.4.0.CR1.xml	2020-01-16 20:15:43.236352	45	EXECUTED	7:6a48ce645a3525488a90fbf76adf3bb3	addColumn tableName=RESOURCE_SERVER_POLICY; addColumn tableName=RESOURCE_SERVER_RESOURCE; addColumn tableName=RESOURCE_SERVER_SCOPE		\N	3.5.4	\N	\N	9205741049
1.8.0	mposolda@redhat.com	META-INF/db2-jpa-changelog-1.8.0.xml	2020-01-16 20:15:42.423358	21	MARK_RAN	7:f987971fe6b37d963bc95fee2b27f8df	addColumn tableName=IDENTITY_PROVIDER; createTable tableName=CLIENT_TEMPLATE; createTable tableName=CLIENT_TEMPLATE_ATTRIBUTES; createTable tableName=TEMPLATE_SCOPE_MAPPING; dropNotNullConstraint columnName=CLIENT_ID, tableName=PROTOCOL_MAPPER; ad...		\N	3.5.4	\N	\N	9205741049
1.8.0-2	keycloak	META-INF/db2-jpa-changelog-1.8.0.xml	2020-01-16 20:15:42.438662	22	MARK_RAN	7:df8bc21027a4f7cbbb01f6344e89ce07	dropDefaultValue columnName=ALGORITHM, tableName=CREDENTIAL; update tableName=CREDENTIAL		\N	3.5.4	\N	\N	9205741049
1.9.0	mposolda@redhat.com	META-INF/jpa-changelog-1.9.0.xml	2020-01-16 20:15:42.467176	23	EXECUTED	7:ed2dc7f799d19ac452cbcda56c929e47	update tableName=REALM; update tableName=REALM; update tableName=REALM; update tableName=REALM; update tableName=CREDENTIAL; update tableName=CREDENTIAL; update tableName=CREDENTIAL; update tableName=REALM; update tableName=REALM; customChange; dr...		\N	3.5.4	\N	\N	9205741049
1.9.1	keycloak	META-INF/jpa-changelog-1.9.1.xml	2020-01-16 20:15:42.475761	24	EXECUTED	7:80b5db88a5dda36ece5f235be8757615	modifyDataType columnName=PRIVATE_KEY, tableName=REALM; modifyDataType columnName=PUBLIC_KEY, tableName=REALM; modifyDataType columnName=CERTIFICATE, tableName=REALM		\N	3.5.4	\N	\N	9205741049
1.9.1	keycloak	META-INF/db2-jpa-changelog-1.9.1.xml	2020-01-16 20:15:42.478916	25	MARK_RAN	7:1437310ed1305a9b93f8848f301726ce	modifyDataType columnName=PRIVATE_KEY, tableName=REALM; modifyDataType columnName=CERTIFICATE, tableName=REALM		\N	3.5.4	\N	\N	9205741049
1.9.2	keycloak	META-INF/jpa-changelog-1.9.2.xml	2020-01-16 20:15:42.538195	26	EXECUTED	7:b82ffb34850fa0836be16deefc6a87c4	createIndex indexName=IDX_USER_EMAIL, tableName=USER_ENTITY; createIndex indexName=IDX_USER_ROLE_MAPPING, tableName=USER_ROLE_MAPPING; createIndex indexName=IDX_USER_GROUP_MAPPING, tableName=USER_GROUP_MEMBERSHIP; createIndex indexName=IDX_USER_CO...		\N	3.5.4	\N	\N	9205741049
authz-2.0.0	psilva@redhat.com	META-INF/jpa-changelog-authz-2.0.0.xml	2020-01-16 20:15:42.658754	27	EXECUTED	7:9cc98082921330d8d9266decdd4bd658	createTable tableName=RESOURCE_SERVER; addPrimaryKey constraintName=CONSTRAINT_FARS, tableName=RESOURCE_SERVER; addUniqueConstraint constraintName=UK_AU8TT6T700S9V50BU18WS5HA6, tableName=RESOURCE_SERVER; createTable tableName=RESOURCE_SERVER_RESOU...		\N	3.5.4	\N	\N	9205741049
authz-2.5.1	psilva@redhat.com	META-INF/jpa-changelog-authz-2.5.1.xml	2020-01-16 20:15:42.663837	28	EXECUTED	7:03d64aeed9cb52b969bd30a7ac0db57e	update tableName=RESOURCE_SERVER_POLICY		\N	3.5.4	\N	\N	9205741049
2.1.0-KEYCLOAK-5461	bburke@redhat.com	META-INF/jpa-changelog-2.1.0.xml	2020-01-16 20:15:42.765278	29	EXECUTED	7:f1f9fd8710399d725b780f463c6b21cd	createTable tableName=BROKER_LINK; createTable tableName=FED_USER_ATTRIBUTE; createTable tableName=FED_USER_CONSENT; createTable tableName=FED_USER_CONSENT_ROLE; createTable tableName=FED_USER_CONSENT_PROT_MAPPER; createTable tableName=FED_USER_CR...		\N	3.5.4	\N	\N	9205741049
2.2.0	bburke@redhat.com	META-INF/jpa-changelog-2.2.0.xml	2020-01-16 20:15:42.803701	30	EXECUTED	7:53188c3eb1107546e6f765835705b6c1	addColumn tableName=ADMIN_EVENT_ENTITY; createTable tableName=CREDENTIAL_ATTRIBUTE; createTable tableName=FED_CREDENTIAL_ATTRIBUTE; modifyDataType columnName=VALUE, tableName=CREDENTIAL; addForeignKeyConstraint baseTableName=FED_CREDENTIAL_ATTRIBU...		\N	3.5.4	\N	\N	9205741049
2.3.0	bburke@redhat.com	META-INF/jpa-changelog-2.3.0.xml	2020-01-16 20:15:42.830212	31	EXECUTED	7:d6e6f3bc57a0c5586737d1351725d4d4	createTable tableName=FEDERATED_USER; addPrimaryKey constraintName=CONSTR_FEDERATED_USER, tableName=FEDERATED_USER; dropDefaultValue columnName=TOTP, tableName=USER_ENTITY; dropColumn columnName=TOTP, tableName=USER_ENTITY; addColumn tableName=IDE...		\N	3.5.4	\N	\N	9205741049
2.4.0	bburke@redhat.com	META-INF/jpa-changelog-2.4.0.xml	2020-01-16 20:15:42.837551	32	EXECUTED	7:454d604fbd755d9df3fd9c6329043aa5	customChange		\N	3.5.4	\N	\N	9205741049
2.5.0	bburke@redhat.com	META-INF/jpa-changelog-2.5.0.xml	2020-01-16 20:15:42.848527	33	EXECUTED	7:57e98a3077e29caf562f7dbf80c72600	customChange; modifyDataType columnName=USER_ID, tableName=OFFLINE_USER_SESSION		\N	3.5.4	\N	\N	9205741049
2.5.0-unicode-oracle	hmlnarik@redhat.com	META-INF/jpa-changelog-2.5.0.xml	2020-01-16 20:15:42.852055	34	MARK_RAN	7:e4c7e8f2256210aee71ddc42f538b57a	modifyDataType columnName=DESCRIPTION, tableName=AUTHENTICATION_FLOW; modifyDataType columnName=DESCRIPTION, tableName=CLIENT_TEMPLATE; modifyDataType columnName=DESCRIPTION, tableName=RESOURCE_SERVER_POLICY; modifyDataType columnName=DESCRIPTION,...		\N	3.5.4	\N	\N	9205741049
2.5.0-unicode-other-dbs	hmlnarik@redhat.com	META-INF/jpa-changelog-2.5.0.xml	2020-01-16 20:15:42.896105	35	EXECUTED	7:09a43c97e49bc626460480aa1379b522	modifyDataType columnName=DESCRIPTION, tableName=AUTHENTICATION_FLOW; modifyDataType columnName=DESCRIPTION, tableName=CLIENT_TEMPLATE; modifyDataType columnName=DESCRIPTION, tableName=RESOURCE_SERVER_POLICY; modifyDataType columnName=DESCRIPTION,...		\N	3.5.4	\N	\N	9205741049
2.5.0-duplicate-email-support	slawomir@dabek.name	META-INF/jpa-changelog-2.5.0.xml	2020-01-16 20:15:42.908178	36	EXECUTED	7:26bfc7c74fefa9126f2ce702fb775553	addColumn tableName=REALM		\N	3.5.4	\N	\N	9205741049
2.5.0-unique-group-names	hmlnarik@redhat.com	META-INF/jpa-changelog-2.5.0.xml	2020-01-16 20:15:42.925696	37	EXECUTED	7:a161e2ae671a9020fff61e996a207377	addUniqueConstraint constraintName=SIBLING_NAMES, tableName=KEYCLOAK_GROUP		\N	3.5.4	\N	\N	9205741049
2.5.1	bburke@redhat.com	META-INF/jpa-changelog-2.5.1.xml	2020-01-16 20:15:42.937441	38	EXECUTED	7:37fc1781855ac5388c494f1442b3f717	addColumn tableName=FED_USER_CONSENT		\N	3.5.4	\N	\N	9205741049
3.0.0	bburke@redhat.com	META-INF/jpa-changelog-3.0.0.xml	2020-01-16 20:15:42.945053	39	EXECUTED	7:13a27db0dae6049541136adad7261d27	addColumn tableName=IDENTITY_PROVIDER		\N	3.5.4	\N	\N	9205741049
3.2.0-fix	keycloak	META-INF/jpa-changelog-3.2.0.xml	2020-01-16 20:15:42.948832	40	MARK_RAN	7:550300617e3b59e8af3a6294df8248a3	addNotNullConstraint columnName=REALM_ID, tableName=CLIENT_INITIAL_ACCESS		\N	3.5.4	\N	\N	9205741049
3.2.0-fix-with-keycloak-5416	keycloak	META-INF/jpa-changelog-3.2.0.xml	2020-01-16 20:15:42.961671	41	MARK_RAN	7:e3a9482b8931481dc2772a5c07c44f17	dropIndex indexName=IDX_CLIENT_INIT_ACC_REALM, tableName=CLIENT_INITIAL_ACCESS; addNotNullConstraint columnName=REALM_ID, tableName=CLIENT_INITIAL_ACCESS; createIndex indexName=IDX_CLIENT_INIT_ACC_REALM, tableName=CLIENT_INITIAL_ACCESS		\N	3.5.4	\N	\N	9205741049
3.2.0-fix-offline-sessions	hmlnarik	META-INF/jpa-changelog-3.2.0.xml	2020-01-16 20:15:42.970956	42	EXECUTED	7:72b07d85a2677cb257edb02b408f332d	customChange		\N	3.5.4	\N	\N	9205741049
3.2.0-fixed	keycloak	META-INF/jpa-changelog-3.2.0.xml	2020-01-16 20:15:43.224998	43	EXECUTED	7:a72a7858967bd414835d19e04d880312	addColumn tableName=REALM; dropPrimaryKey constraintName=CONSTRAINT_OFFL_CL_SES_PK2, tableName=OFFLINE_CLIENT_SESSION; dropColumn columnName=CLIENT_SESSION_ID, tableName=OFFLINE_CLIENT_SESSION; addPrimaryKey constraintName=CONSTRAINT_OFFL_CL_SES_P...		\N	3.5.4	\N	\N	9205741049
3.3.0	keycloak	META-INF/jpa-changelog-3.3.0.xml	2020-01-16 20:15:43.231076	44	EXECUTED	7:94edff7cf9ce179e7e85f0cd78a3cf2c	addColumn tableName=USER_ENTITY		\N	3.5.4	\N	\N	9205741049
authz-3.4.0.CR1-resource-server-pk-change-part2-KEYCLOAK-6095	hmlnarik@redhat.com	META-INF/jpa-changelog-authz-3.4.0.CR1.xml	2020-01-16 20:15:43.246913	46	EXECUTED	7:e64b5dcea7db06077c6e57d3b9e5ca14	customChange		\N	3.5.4	\N	\N	9205741049
authz-3.4.0.CR1-resource-server-pk-change-part3-fixed	glavoie@gmail.com	META-INF/jpa-changelog-authz-3.4.0.CR1.xml	2020-01-16 20:15:43.250133	47	MARK_RAN	7:fd8cf02498f8b1e72496a20afc75178c	dropIndex indexName=IDX_RES_SERV_POL_RES_SERV, tableName=RESOURCE_SERVER_POLICY; dropIndex indexName=IDX_RES_SRV_RES_RES_SRV, tableName=RESOURCE_SERVER_RESOURCE; dropIndex indexName=IDX_RES_SRV_SCOPE_RES_SRV, tableName=RESOURCE_SERVER_SCOPE		\N	3.5.4	\N	\N	9205741049
authz-3.4.0.CR1-resource-server-pk-change-part3-fixed-nodropindex	glavoie@gmail.com	META-INF/jpa-changelog-authz-3.4.0.CR1.xml	2020-01-16 20:15:43.300548	48	EXECUTED	7:542794f25aa2b1fbabb7e577d6646319	addNotNullConstraint columnName=RESOURCE_SERVER_CLIENT_ID, tableName=RESOURCE_SERVER_POLICY; addNotNullConstraint columnName=RESOURCE_SERVER_CLIENT_ID, tableName=RESOURCE_SERVER_RESOURCE; addNotNullConstraint columnName=RESOURCE_SERVER_CLIENT_ID, ...		\N	3.5.4	\N	\N	9205741049
authn-3.4.0.CR1-refresh-token-max-reuse	glavoie@gmail.com	META-INF/jpa-changelog-authz-3.4.0.CR1.xml	2020-01-16 20:15:43.306623	49	EXECUTED	7:edad604c882df12f74941dac3cc6d650	addColumn tableName=REALM		\N	3.5.4	\N	\N	9205741049
3.4.0	keycloak	META-INF/jpa-changelog-3.4.0.xml	2020-01-16 20:15:43.380076	50	EXECUTED	7:0f88b78b7b46480eb92690cbf5e44900	addPrimaryKey constraintName=CONSTRAINT_REALM_DEFAULT_ROLES, tableName=REALM_DEFAULT_ROLES; addPrimaryKey constraintName=CONSTRAINT_COMPOSITE_ROLE, tableName=COMPOSITE_ROLE; addPrimaryKey constraintName=CONSTR_REALM_DEFAULT_GROUPS, tableName=REALM...		\N	3.5.4	\N	\N	9205741049
3.4.0-KEYCLOAK-5230	hmlnarik@redhat.com	META-INF/jpa-changelog-3.4.0.xml	2020-01-16 20:15:43.442623	51	EXECUTED	7:d560e43982611d936457c327f872dd59	createIndex indexName=IDX_FU_ATTRIBUTE, tableName=FED_USER_ATTRIBUTE; createIndex indexName=IDX_FU_CONSENT, tableName=FED_USER_CONSENT; createIndex indexName=IDX_FU_CONSENT_RU, tableName=FED_USER_CONSENT; createIndex indexName=IDX_FU_CREDENTIAL, t...		\N	3.5.4	\N	\N	9205741049
3.4.1	psilva@redhat.com	META-INF/jpa-changelog-3.4.1.xml	2020-01-16 20:15:43.447514	52	EXECUTED	7:c155566c42b4d14ef07059ec3b3bbd8e	modifyDataType columnName=VALUE, tableName=CLIENT_ATTRIBUTES		\N	3.5.4	\N	\N	9205741049
3.4.2	keycloak	META-INF/jpa-changelog-3.4.2.xml	2020-01-16 20:15:43.45142	53	EXECUTED	7:b40376581f12d70f3c89ba8ddf5b7dea	update tableName=REALM		\N	3.5.4	\N	\N	9205741049
3.4.2-KEYCLOAK-5172	mkanis@redhat.com	META-INF/jpa-changelog-3.4.2.xml	2020-01-16 20:15:43.45542	54	EXECUTED	7:a1132cc395f7b95b3646146c2e38f168	update tableName=CLIENT		\N	3.5.4	\N	\N	9205741049
4.0.0-KEYCLOAK-6335	bburke@redhat.com	META-INF/jpa-changelog-4.0.0.xml	2020-01-16 20:15:43.465758	55	EXECUTED	7:d8dc5d89c789105cfa7ca0e82cba60af	createTable tableName=CLIENT_AUTH_FLOW_BINDINGS; addPrimaryKey constraintName=C_CLI_FLOW_BIND, tableName=CLIENT_AUTH_FLOW_BINDINGS		\N	3.5.4	\N	\N	9205741049
4.0.0-CLEANUP-UNUSED-TABLE	bburke@redhat.com	META-INF/jpa-changelog-4.0.0.xml	2020-01-16 20:15:43.472337	56	EXECUTED	7:7822e0165097182e8f653c35517656a3	dropTable tableName=CLIENT_IDENTITY_PROV_MAPPING		\N	3.5.4	\N	\N	9205741049
4.0.0-KEYCLOAK-6228	bburke@redhat.com	META-INF/jpa-changelog-4.0.0.xml	2020-01-16 20:15:43.503077	57	EXECUTED	7:c6538c29b9c9a08f9e9ea2de5c2b6375	dropUniqueConstraint constraintName=UK_JKUWUVD56ONTGSUHOGM8UEWRT, tableName=USER_CONSENT; dropNotNullConstraint columnName=CLIENT_ID, tableName=USER_CONSENT; addColumn tableName=USER_CONSENT; addUniqueConstraint constraintName=UK_JKUWUVD56ONTGSUHO...		\N	3.5.4	\N	\N	9205741049
4.0.0-KEYCLOAK-5579-fixed	mposolda@redhat.com	META-INF/jpa-changelog-4.0.0.xml	2020-01-16 20:15:43.642294	58	EXECUTED	7:6d4893e36de22369cf73bcb051ded875	dropForeignKeyConstraint baseTableName=CLIENT_TEMPLATE_ATTRIBUTES, constraintName=FK_CL_TEMPL_ATTR_TEMPL; renameTable newTableName=CLIENT_SCOPE_ATTRIBUTES, oldTableName=CLIENT_TEMPLATE_ATTRIBUTES; renameColumn newColumnName=SCOPE_ID, oldColumnName...		\N	3.5.4	\N	\N	9205741049
authz-4.0.0.CR1	psilva@redhat.com	META-INF/jpa-changelog-authz-4.0.0.CR1.xml	2020-01-16 20:15:43.679334	59	EXECUTED	7:57960fc0b0f0dd0563ea6f8b2e4a1707	createTable tableName=RESOURCE_SERVER_PERM_TICKET; addPrimaryKey constraintName=CONSTRAINT_FAPMT, tableName=RESOURCE_SERVER_PERM_TICKET; addForeignKeyConstraint baseTableName=RESOURCE_SERVER_PERM_TICKET, constraintName=FK_FRSRHO213XCX4WNKOG82SSPMT...		\N	3.5.4	\N	\N	9205741049
authz-4.0.0.Beta3	psilva@redhat.com	META-INF/jpa-changelog-authz-4.0.0.Beta3.xml	2020-01-16 20:15:43.687764	60	EXECUTED	7:2b4b8bff39944c7097977cc18dbceb3b	addColumn tableName=RESOURCE_SERVER_POLICY; addColumn tableName=RESOURCE_SERVER_PERM_TICKET; addForeignKeyConstraint baseTableName=RESOURCE_SERVER_PERM_TICKET, constraintName=FK_FRSRPO2128CX4WNKOG82SSRFY, referencedTableName=RESOURCE_SERVER_POLICY		\N	3.5.4	\N	\N	9205741049
authz-4.2.0.Final	mhajas@redhat.com	META-INF/jpa-changelog-authz-4.2.0.Final.xml	2020-01-16 20:15:43.697412	61	EXECUTED	7:2aa42a964c59cd5b8ca9822340ba33a8	createTable tableName=RESOURCE_URIS; addForeignKeyConstraint baseTableName=RESOURCE_URIS, constraintName=FK_RESOURCE_SERVER_URIS, referencedTableName=RESOURCE_SERVER_RESOURCE; customChange; dropColumn columnName=URI, tableName=RESOURCE_SERVER_RESO...		\N	3.5.4	\N	\N	9205741049
authz-4.2.0.Final-KEYCLOAK-9944	hmlnarik@redhat.com	META-INF/jpa-changelog-authz-4.2.0.Final.xml	2020-01-16 20:15:43.707602	62	EXECUTED	7:9ac9e58545479929ba23f4a3087a0346	addPrimaryKey constraintName=CONSTRAINT_RESOUR_URIS_PK, tableName=RESOURCE_URIS		\N	3.5.4	\N	\N	9205741049
4.2.0-KEYCLOAK-6313	wadahiro@gmail.com	META-INF/jpa-changelog-4.2.0.xml	2020-01-16 20:15:43.712571	63	EXECUTED	7:14d407c35bc4fe1976867756bcea0c36	addColumn tableName=REQUIRED_ACTION_PROVIDER		\N	3.5.4	\N	\N	9205741049
4.3.0-KEYCLOAK-7984	wadahiro@gmail.com	META-INF/jpa-changelog-4.3.0.xml	2020-01-16 20:15:43.71625	64	EXECUTED	7:241a8030c748c8548e346adee548fa93	update tableName=REQUIRED_ACTION_PROVIDER		\N	3.5.4	\N	\N	9205741049
4.6.0-KEYCLOAK-7950	psilva@redhat.com	META-INF/jpa-changelog-4.6.0.xml	2020-01-16 20:15:43.719773	65	EXECUTED	7:7d3182f65a34fcc61e8d23def037dc3f	update tableName=RESOURCE_SERVER_RESOURCE		\N	3.5.4	\N	\N	9205741049
4.6.0-KEYCLOAK-8377	keycloak	META-INF/jpa-changelog-4.6.0.xml	2020-01-16 20:15:43.73922	66	EXECUTED	7:b30039e00a0b9715d430d1b0636728fa	createTable tableName=ROLE_ATTRIBUTE; addPrimaryKey constraintName=CONSTRAINT_ROLE_ATTRIBUTE_PK, tableName=ROLE_ATTRIBUTE; addForeignKeyConstraint baseTableName=ROLE_ATTRIBUTE, constraintName=FK_ROLE_ATTRIBUTE_ID, referencedTableName=KEYCLOAK_ROLE...		\N	3.5.4	\N	\N	9205741049
4.6.0-KEYCLOAK-8555	gideonray@gmail.com	META-INF/jpa-changelog-4.6.0.xml	2020-01-16 20:15:43.748542	67	EXECUTED	7:3797315ca61d531780f8e6f82f258159	createIndex indexName=IDX_COMPONENT_PROVIDER_TYPE, tableName=COMPONENT		\N	3.5.4	\N	\N	9205741049
4.7.0-KEYCLOAK-1267	sguilhen@redhat.com	META-INF/jpa-changelog-4.7.0.xml	2020-01-16 20:15:43.754467	68	EXECUTED	7:c7aa4c8d9573500c2d347c1941ff0301	addColumn tableName=REALM		\N	3.5.4	\N	\N	9205741049
4.7.0-KEYCLOAK-7275	keycloak	META-INF/jpa-changelog-4.7.0.xml	2020-01-16 20:15:43.771961	69	EXECUTED	7:b207faee394fc074a442ecd42185a5dd	renameColumn newColumnName=CREATED_ON, oldColumnName=LAST_SESSION_REFRESH, tableName=OFFLINE_USER_SESSION; addNotNullConstraint columnName=CREATED_ON, tableName=OFFLINE_USER_SESSION; addColumn tableName=OFFLINE_USER_SESSION; customChange; createIn...		\N	3.5.4	\N	\N	9205741049
4.8.0-KEYCLOAK-8835	sguilhen@redhat.com	META-INF/jpa-changelog-4.8.0.xml	2020-01-16 20:15:43.778945	70	EXECUTED	7:ab9a9762faaba4ddfa35514b212c4922	addNotNullConstraint columnName=SSO_MAX_LIFESPAN_REMEMBER_ME, tableName=REALM; addNotNullConstraint columnName=SSO_IDLE_TIMEOUT_REMEMBER_ME, tableName=REALM		\N	3.5.4	\N	\N	9205741049
authz-7.0.0-KEYCLOAK-10443	psilva@redhat.com	META-INF/jpa-changelog-authz-7.0.0.xml	2020-01-16 20:15:43.784866	71	EXECUTED	7:b9710f74515a6ccb51b72dc0d19df8c4	addColumn tableName=RESOURCE_SERVER		\N	3.5.4	\N	\N	9205741049
8.0.0-adding-credential-columns	keycloak	META-INF/jpa-changelog-8.0.0.xml	2020-01-16 20:15:43.792738	72	EXECUTED	7:ec9707ae4d4f0b7452fee20128083879	addColumn tableName=CREDENTIAL; addColumn tableName=FED_USER_CREDENTIAL		\N	3.5.4	\N	\N	9205741049
8.0.0-updating-credential-data-not-oracle	keycloak	META-INF/jpa-changelog-8.0.0.xml	2020-01-16 20:15:43.799546	73	EXECUTED	7:03b3f4b264c3c68ba082250a80b74216	update tableName=CREDENTIAL; update tableName=CREDENTIAL; update tableName=CREDENTIAL; update tableName=FED_USER_CREDENTIAL; update tableName=FED_USER_CREDENTIAL; update tableName=FED_USER_CREDENTIAL		\N	3.5.4	\N	\N	9205741049
8.0.0-updating-credential-data-oracle	keycloak	META-INF/jpa-changelog-8.0.0.xml	2020-01-16 20:15:43.802344	74	MARK_RAN	7:64c5728f5ca1f5aa4392217701c4fe23	update tableName=CREDENTIAL; update tableName=CREDENTIAL; update tableName=CREDENTIAL; update tableName=FED_USER_CREDENTIAL; update tableName=FED_USER_CREDENTIAL; update tableName=FED_USER_CREDENTIAL		\N	3.5.4	\N	\N	9205741049
8.0.0-credential-cleanup	keycloak	META-INF/jpa-changelog-8.0.0.xml	2020-01-16 20:15:43.819191	75	EXECUTED	7:41f3566ac5177459e1ed3ce8f0ad35d2	dropDefaultValue columnName=COUNTER, tableName=CREDENTIAL; dropDefaultValue columnName=DIGITS, tableName=CREDENTIAL; dropDefaultValue columnName=PERIOD, tableName=CREDENTIAL; dropDefaultValue columnName=ALGORITHM, tableName=CREDENTIAL; dropColumn ...		\N	3.5.4	\N	\N	9205741049
8.0.0-resource-tag-support	keycloak	META-INF/jpa-changelog-8.0.0.xml	2020-01-16 20:15:43.829644	76	EXECUTED	7:a73379915c23bfad3e8f5c6d5c0aa4bd	addColumn tableName=MIGRATION_MODEL; createIndex indexName=IDX_UPDATE_TIME, tableName=MIGRATION_MODEL		\N	3.5.4	\N	\N	9205741049
\.


--
-- Data for Name: databasechangeloglock; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.databasechangeloglock (id, locked, lockgranted, lockedby) FROM stdin;
1	f	\N	\N
1000	f	\N	\N
1001	f	\N	\N
\.


--
-- Data for Name: default_client_scope; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.default_client_scope (realm_id, scope_id, default_scope) FROM stdin;
master	02523afa-a48d-410f-806d-9424a06258a1	f
master	c8ee013d-6f5f-4537-abfc-223d033cfdef	t
master	f9fec2d6-19ad-4058-9899-783805a982e8	t
master	655b6977-f29c-49f7-a4a8-8879c90c326f	t
master	98d28782-fa3a-4379-99e8-4ae7e8a29e2d	f
master	c299f45b-9e42-4d79-94f3-2d63c4ad2b6c	f
master	4924be0a-fffa-4c2f-8975-dd98138ae7a5	t
master	e0f4afb4-705c-4de2-b536-025e1522e270	t
master	9a266253-af14-4478-8a65-33070b82df1a	f
ModMappings	44e568c9-e763-4a8f-979f-4b12932fe0dc	f
ModMappings	05ef0639-aae9-4a1f-ad43-b8f6d1eb0a50	t
ModMappings	30af825f-1ec9-45af-a678-279a6458f78d	t
ModMappings	61b57419-d020-4777-a159-e3b13122842e	t
ModMappings	880bb5d8-829b-4e64-b07d-913dab25091a	f
ModMappings	2d9c89c5-4764-48eb-9140-c34aceb2b1c7	f
ModMappings	88f3f2e7-c4fd-4439-a3a3-8fab2c64bd03	t
ModMappings	e682941a-b6e0-4a6a-8539-150b36cb85e9	t
ModMappings	b9cbe9c2-5da1-4be4-a1e5-c308e6b222b3	f
\.


--
-- Data for Name: event_entity; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.event_entity (id, client_id, details_json, error, ip_address, realm_id, session_id, event_time, type, user_id) FROM stdin;
\.


--
-- Data for Name: fed_user_attribute; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.fed_user_attribute (id, name, user_id, realm_id, storage_provider_id, value) FROM stdin;
\.


--
-- Data for Name: fed_user_consent; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.fed_user_consent (id, client_id, user_id, realm_id, storage_provider_id, created_date, last_updated_date, client_storage_provider, external_client_id) FROM stdin;
\.


--
-- Data for Name: fed_user_consent_cl_scope; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.fed_user_consent_cl_scope (user_consent_id, scope_id) FROM stdin;
\.


--
-- Data for Name: fed_user_credential; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.fed_user_credential (id, salt, type, created_date, user_id, realm_id, storage_provider_id, user_label, secret_data, credential_data, priority) FROM stdin;
\.


--
-- Data for Name: fed_user_group_membership; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.fed_user_group_membership (group_id, user_id, realm_id, storage_provider_id) FROM stdin;
\.


--
-- Data for Name: fed_user_required_action; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.fed_user_required_action (required_action, user_id, realm_id, storage_provider_id) FROM stdin;
\.


--
-- Data for Name: fed_user_role_mapping; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.fed_user_role_mapping (role_id, user_id, realm_id, storage_provider_id) FROM stdin;
\.


--
-- Data for Name: federated_identity; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.federated_identity (identity_provider, realm_id, federated_user_id, federated_username, token, user_id) FROM stdin;
\.


--
-- Data for Name: federated_user; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.federated_user (id, storage_provider_id, realm_id) FROM stdin;
\.


--
-- Data for Name: group_attribute; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.group_attribute (id, name, value, group_id) FROM stdin;
\.


--
-- Data for Name: group_role_mapping; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.group_role_mapping (role_id, group_id) FROM stdin;
\.


--
-- Data for Name: identity_provider; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.identity_provider (internal_id, enabled, provider_alias, provider_id, store_token, authenticate_by_default, realm_id, add_token_role, trust_email, first_broker_login_flow_id, post_broker_login_flow_id, provider_display_name, link_only) FROM stdin;
\.


--
-- Data for Name: identity_provider_config; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.identity_provider_config (identity_provider_id, value, name) FROM stdin;
\.


--
-- Data for Name: identity_provider_mapper; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.identity_provider_mapper (id, name, idp_alias, idp_mapper_name, realm_id) FROM stdin;
\.


--
-- Data for Name: idp_mapper_config; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.idp_mapper_config (idp_mapper_id, value, name) FROM stdin;
\.


--
-- Data for Name: keycloak_group; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.keycloak_group (id, name, parent_group, realm_id) FROM stdin;
\.


--
-- Data for Name: keycloak_role; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.keycloak_role (id, client_realm_constraint, client_role, description, name, realm_id, client, realm) FROM stdin;
b21192ad-92c5-4b28-a87b-361e66aab625	master	f	${role_admin}	admin	master	\N	master
1f73a5d6-fd8c-4015-b578-8df644251959	master	f	${role_create-realm}	create-realm	master	\N	master
880bf334-f8bf-4735-877c-0f0eff7970b8	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_create-client}	create-client	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
61fde2d6-e74f-4cd5-b274-2f22c913db0a	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_view-realm}	view-realm	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
e9a7ed8d-81ea-4d12-a2a8-3bcae99817e7	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_view-users}	view-users	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
fff8f672-002a-4639-85ca-556bc14ea62d	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_view-clients}	view-clients	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
526a4da7-73de-4d15-8b95-8c36bd35c7b5	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_view-events}	view-events	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
8339383b-96f0-4a93-a227-03a798dca271	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_view-identity-providers}	view-identity-providers	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
360adbb1-fe02-43f2-bf7f-ea4d0aa9a45c	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_view-authorization}	view-authorization	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
33ce929f-7839-4c63-883e-3f9e1bbedf83	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_manage-realm}	manage-realm	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
d308da3e-e9c3-4b05-8a76-b0ef249eb2c1	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_manage-users}	manage-users	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
5d4aeb28-ce92-4612-8e38-54ccf2785abc	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_manage-clients}	manage-clients	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
d61445e7-b340-429c-bcc7-c71a7b463c1f	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_manage-events}	manage-events	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
4f2c31f3-89af-408d-9a72-ff8dd2e93b04	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_manage-identity-providers}	manage-identity-providers	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
6c627d35-f93c-4ca8-a7b9-1d2f728bf097	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_manage-authorization}	manage-authorization	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
2d183fc2-adf3-4a3f-86e7-2d03a37d93ac	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_query-users}	query-users	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
6cf50abb-2c1e-49f6-b5f2-c7f2afce2ce9	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_query-clients}	query-clients	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
60fbec97-898d-4d3b-9327-9281394efb2f	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_query-realms}	query-realms	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
d5d26a2f-9ad2-49d5-9667-18d38221bff6	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_query-groups}	query-groups	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
f2413a59-2051-4fde-85a2-6c4b62d4d6b6	c5f82688-a1d0-4d07-96f3-6dd303a5bd61	t	${role_view-profile}	view-profile	master	c5f82688-a1d0-4d07-96f3-6dd303a5bd61	\N
7ffebb0e-73d8-4f0b-acee-44b682519130	c5f82688-a1d0-4d07-96f3-6dd303a5bd61	t	${role_manage-account}	manage-account	master	c5f82688-a1d0-4d07-96f3-6dd303a5bd61	\N
c340dd29-1fcc-4fe4-9b5d-f1d3ae28b350	c5f82688-a1d0-4d07-96f3-6dd303a5bd61	t	${role_manage-account-links}	manage-account-links	master	c5f82688-a1d0-4d07-96f3-6dd303a5bd61	\N
f163fe00-bcb0-4b4c-87aa-ff518f92aacc	e9c4b2a4-e48b-477a-b5f1-3e911fce718d	t	${role_read-token}	read-token	master	e9c4b2a4-e48b-477a-b5f1-3e911fce718d	\N
261fe951-b52c-4525-9b3b-6413cc75d1e8	6e4abf2f-cad7-4035-8014-f7b86990dee7	t	${role_impersonation}	impersonation	master	6e4abf2f-cad7-4035-8014-f7b86990dee7	\N
5726c14b-12e1-4acf-ab64-a55d60b8c582	master	f	${role_offline-access}	offline_access	master	\N	master
06b6040e-2d97-4054-92f6-aab17984a8fe	master	f	${role_uma_authorization}	uma_authorization	master	\N	master
ad64e08c-fb68-40d7-9b6d-f90d334942ff	b1781787-3740-49f7-9015-51485579c1e5	t	${role_create-client}	create-client	master	b1781787-3740-49f7-9015-51485579c1e5	\N
3596a222-eb39-4f9d-9693-023334d1d771	b1781787-3740-49f7-9015-51485579c1e5	t	${role_view-realm}	view-realm	master	b1781787-3740-49f7-9015-51485579c1e5	\N
3fe24019-5824-4653-ba77-4e52bd81b704	b1781787-3740-49f7-9015-51485579c1e5	t	${role_view-users}	view-users	master	b1781787-3740-49f7-9015-51485579c1e5	\N
3dfcf3c3-ea9b-4f4c-a5cd-72b3d69581b4	b1781787-3740-49f7-9015-51485579c1e5	t	${role_view-clients}	view-clients	master	b1781787-3740-49f7-9015-51485579c1e5	\N
c486ee7e-d3e0-413c-88ef-3ebd8d9dadd8	b1781787-3740-49f7-9015-51485579c1e5	t	${role_view-events}	view-events	master	b1781787-3740-49f7-9015-51485579c1e5	\N
748064a7-2e42-4bff-acca-dad4308a1bb8	b1781787-3740-49f7-9015-51485579c1e5	t	${role_view-identity-providers}	view-identity-providers	master	b1781787-3740-49f7-9015-51485579c1e5	\N
d1320b12-3020-4e85-94a3-faff695a2659	b1781787-3740-49f7-9015-51485579c1e5	t	${role_view-authorization}	view-authorization	master	b1781787-3740-49f7-9015-51485579c1e5	\N
735c6239-5c3e-41a1-bb60-11e7046927e5	b1781787-3740-49f7-9015-51485579c1e5	t	${role_manage-realm}	manage-realm	master	b1781787-3740-49f7-9015-51485579c1e5	\N
8251c5fb-14ad-4f4b-ba69-2a6802dace79	b1781787-3740-49f7-9015-51485579c1e5	t	${role_manage-users}	manage-users	master	b1781787-3740-49f7-9015-51485579c1e5	\N
967b044d-2b0e-4d75-b3a8-675be1f522d0	b1781787-3740-49f7-9015-51485579c1e5	t	${role_manage-clients}	manage-clients	master	b1781787-3740-49f7-9015-51485579c1e5	\N
3fb9fe09-94c8-46d0-b517-c930df083a49	b1781787-3740-49f7-9015-51485579c1e5	t	${role_manage-events}	manage-events	master	b1781787-3740-49f7-9015-51485579c1e5	\N
3e80cdf9-9c5f-40bf-a3b7-7cb080d7c3ba	b1781787-3740-49f7-9015-51485579c1e5	t	${role_manage-identity-providers}	manage-identity-providers	master	b1781787-3740-49f7-9015-51485579c1e5	\N
48e84a51-25e0-44ce-931a-c8b6b78ca209	b1781787-3740-49f7-9015-51485579c1e5	t	${role_manage-authorization}	manage-authorization	master	b1781787-3740-49f7-9015-51485579c1e5	\N
2e189751-c8ca-4786-8ae1-7033a8b349fa	b1781787-3740-49f7-9015-51485579c1e5	t	${role_query-users}	query-users	master	b1781787-3740-49f7-9015-51485579c1e5	\N
b6c9db9b-812f-4023-9bc8-3169885ac47d	b1781787-3740-49f7-9015-51485579c1e5	t	${role_query-clients}	query-clients	master	b1781787-3740-49f7-9015-51485579c1e5	\N
386fd497-4cd6-4d38-9e36-40909dcd3052	b1781787-3740-49f7-9015-51485579c1e5	t	${role_query-realms}	query-realms	master	b1781787-3740-49f7-9015-51485579c1e5	\N
aaf59ef8-723f-4b4b-8ff3-b2895d9fe09d	b1781787-3740-49f7-9015-51485579c1e5	t	${role_query-groups}	query-groups	master	b1781787-3740-49f7-9015-51485579c1e5	\N
b0bc8ff0-764b-4347-b08a-e227401183df	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_realm-admin}	realm-admin	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
e4142a3a-5a3f-42a4-b311-91e4b997f223	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_create-client}	create-client	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
5291fb1a-46f5-4029-9a9e-9a23952110c4	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_view-realm}	view-realm	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
8e0a2aa0-41be-4fed-b9db-18eac27f9c78	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_view-users}	view-users	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
aa9c3144-7c50-4db6-b088-ebcb7206266f	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_view-clients}	view-clients	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
fc19eb56-00a7-416e-a95e-e44eea129a45	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_view-events}	view-events	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
ce080e12-e6a1-416f-a323-9e44000f1961	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_view-identity-providers}	view-identity-providers	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
6915a553-228e-440d-9772-e994f9084477	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_view-authorization}	view-authorization	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
ae720494-acd5-4c06-bb9c-a7381e2b815f	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_manage-realm}	manage-realm	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
4f41266f-9074-41e7-9e77-f26adf921217	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_manage-users}	manage-users	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
84c30bf0-db52-47b8-aeff-22738a3face6	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_manage-clients}	manage-clients	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
f3f2bf16-7162-4f89-bb51-1b5ef45aade2	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_manage-events}	manage-events	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
f7e5962d-1290-4939-97fd-6e20bcd7996c	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_manage-identity-providers}	manage-identity-providers	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
d77fe98d-a72e-4430-85d4-7cd18dcc5aa8	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_manage-authorization}	manage-authorization	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
df264a00-54ff-41e3-af59-74e84d162f6f	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_query-users}	query-users	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
d19fa9aa-0321-4c47-86db-2a4c3913bc29	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_query-clients}	query-clients	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
86dcf10c-f726-438e-83ed-9ccf16f83553	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_query-realms}	query-realms	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
7bd8cd76-e01a-49d4-b72f-ce0b6b2bcce3	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_query-groups}	query-groups	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
9e9c4679-a175-4a04-b72b-234686a133b6	b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	t	${role_view-profile}	view-profile	ModMappings	b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	\N
625aa352-faf9-4bfe-99f1-5f270200eb75	b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	t	${role_manage-account}	manage-account	ModMappings	b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	\N
6c425314-8bd4-417f-a624-685a3ea2fa07	b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	t	${role_manage-account-links}	manage-account-links	ModMappings	b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	\N
022eba55-ca1f-4037-9b7c-a0dc234e4484	b1781787-3740-49f7-9015-51485579c1e5	t	${role_impersonation}	impersonation	master	b1781787-3740-49f7-9015-51485579c1e5	\N
5b33f0b7-bc80-483f-a102-6be01ae64c30	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	t	${role_impersonation}	impersonation	ModMappings	d96a45e8-8ab2-4b5e-98ee-4a20403a11f8	\N
2e7cf518-43e4-44c7-95da-de844371b5fd	f9564792-f154-4177-ac4c-0d47d2f4c026	t	${role_read-token}	read-token	ModMappings	f9564792-f154-4177-ac4c-0d47d2f4c026	\N
770e4d48-d5a7-4795-8818-af03c8cf2d14	ModMappings	f	${role_offline-access}	offline_access	ModMappings	\N	ModMappings
c82225bf-240c-444a-b715-57bb9229718f	ModMappings	f	${role_uma_authorization}	uma_authorization	ModMappings	\N	ModMappings
6603554d-5faf-4593-ba06-8375360ff166	aed69ab6-ca48-4b7f-a94d-629361b04bb8	t	\N	uma_protection	ModMappings	aed69ab6-ca48-4b7f-a94d-629361b04bb8	\N
3256738e-6f4c-4a0f-bc37-01655a34978b	aed69ab6-ca48-4b7f-a94d-629361b04bb8	t	\N	USER	ModMappings	aed69ab6-ca48-4b7f-a94d-629361b04bb8	\N
d69ffc6d-4e3f-4adf-a233-ee8f55fab822	ModMappings	f	\N	USER	ModMappings	\N	ModMappings
\.


--
-- Data for Name: migration_model; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.migration_model (id, version, update_time) FROM stdin;
jdnow	8.0.1	1579205747
\.


--
-- Data for Name: offline_client_session; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.offline_client_session (user_session_id, client_id, offline_flag, "timestamp", data, client_storage_provider, external_client_id) FROM stdin;
\.


--
-- Data for Name: offline_user_session; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.offline_user_session (user_session_id, user_id, realm_id, created_on, offline_flag, data, last_session_refresh) FROM stdin;
\.


--
-- Data for Name: policy_config; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.policy_config (policy_id, name, value) FROM stdin;
22d389b9-62a8-481e-87be-7d38f901a574	code	// by default, grants any permission associated with this policy\n$evaluation.grant();\n
2fa44c63-4ea8-4438-a6fa-2b7f4306a307	defaultResourceType	urn:api:resources:default
\.


--
-- Data for Name: protocol_mapper; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.protocol_mapper (id, name, protocol, protocol_mapper_name, client_id, client_scope_id) FROM stdin;
2281d397-4263-462b-bb83-7b7c5c7d50d6	locale	openid-connect	oidc-usermodel-attribute-mapper	24463fff-1058-4fc9-8c1e-9ecbcf7fa2e9	\N
cd333b14-9da9-49d2-ac24-c4cf14c386fa	role list	saml	saml-role-list-mapper	\N	c8ee013d-6f5f-4537-abfc-223d033cfdef
02954db9-69d2-4e36-b6f8-602534985614	full name	openid-connect	oidc-full-name-mapper	\N	f9fec2d6-19ad-4058-9899-783805a982e8
1d7a0e05-8fad-4f38-a4ed-a634f0bf868b	family name	openid-connect	oidc-usermodel-property-mapper	\N	f9fec2d6-19ad-4058-9899-783805a982e8
3dee66f3-af8c-42aa-bcdb-c9c0a8a4a35b	given name	openid-connect	oidc-usermodel-property-mapper	\N	f9fec2d6-19ad-4058-9899-783805a982e8
d0ea70be-cddc-41c7-b95e-41f654f791d7	middle name	openid-connect	oidc-usermodel-attribute-mapper	\N	f9fec2d6-19ad-4058-9899-783805a982e8
5d0571c0-5be2-4ca4-b311-ec4e30261848	nickname	openid-connect	oidc-usermodel-attribute-mapper	\N	f9fec2d6-19ad-4058-9899-783805a982e8
3045cad8-9230-424d-83a2-9c96ff6688dd	username	openid-connect	oidc-usermodel-property-mapper	\N	f9fec2d6-19ad-4058-9899-783805a982e8
821e7296-f3a3-4c9c-b29b-30e3765272e8	profile	openid-connect	oidc-usermodel-attribute-mapper	\N	f9fec2d6-19ad-4058-9899-783805a982e8
d1949eb1-1a52-42e7-93a7-f74ad96577dc	picture	openid-connect	oidc-usermodel-attribute-mapper	\N	f9fec2d6-19ad-4058-9899-783805a982e8
2d936bdd-c084-4221-9a91-32c71d3996d4	website	openid-connect	oidc-usermodel-attribute-mapper	\N	f9fec2d6-19ad-4058-9899-783805a982e8
91b6ef24-01bf-471b-89c7-55d86e4a682f	gender	openid-connect	oidc-usermodel-attribute-mapper	\N	f9fec2d6-19ad-4058-9899-783805a982e8
26d600f6-93a9-4556-aab4-b49feafef2da	birthdate	openid-connect	oidc-usermodel-attribute-mapper	\N	f9fec2d6-19ad-4058-9899-783805a982e8
84512d85-5c7d-452e-92b6-08583b95a10a	zoneinfo	openid-connect	oidc-usermodel-attribute-mapper	\N	f9fec2d6-19ad-4058-9899-783805a982e8
520727d3-23e8-44bf-8151-8c1a88c66278	locale	openid-connect	oidc-usermodel-attribute-mapper	\N	f9fec2d6-19ad-4058-9899-783805a982e8
158a0943-9e13-480f-8e0b-95342d96d12a	updated at	openid-connect	oidc-usermodel-attribute-mapper	\N	f9fec2d6-19ad-4058-9899-783805a982e8
203edf52-38f8-49fc-8810-a46cb2bdc7c4	email	openid-connect	oidc-usermodel-property-mapper	\N	655b6977-f29c-49f7-a4a8-8879c90c326f
7b959da5-d4de-4e89-bf5c-c3bad0f1248e	email verified	openid-connect	oidc-usermodel-property-mapper	\N	655b6977-f29c-49f7-a4a8-8879c90c326f
14850045-d90f-4758-a71a-ea32373411ba	address	openid-connect	oidc-address-mapper	\N	98d28782-fa3a-4379-99e8-4ae7e8a29e2d
178eb17c-fc50-4efb-a1b7-e860da413a06	phone number	openid-connect	oidc-usermodel-attribute-mapper	\N	c299f45b-9e42-4d79-94f3-2d63c4ad2b6c
bf5faaac-26da-4238-9d7c-8166faffe683	phone number verified	openid-connect	oidc-usermodel-attribute-mapper	\N	c299f45b-9e42-4d79-94f3-2d63c4ad2b6c
05e98394-df87-4448-b3ce-27a9b31d76df	realm roles	openid-connect	oidc-usermodel-realm-role-mapper	\N	4924be0a-fffa-4c2f-8975-dd98138ae7a5
939cd1ba-6df6-4624-b8ce-59357bc133ba	client roles	openid-connect	oidc-usermodel-client-role-mapper	\N	4924be0a-fffa-4c2f-8975-dd98138ae7a5
39dd187f-2efa-4bf6-9233-a242bd376710	audience resolve	openid-connect	oidc-audience-resolve-mapper	\N	4924be0a-fffa-4c2f-8975-dd98138ae7a5
f2877514-b88a-4639-b8ac-55c37e2f3fde	allowed web origins	openid-connect	oidc-allowed-origins-mapper	\N	e0f4afb4-705c-4de2-b536-025e1522e270
4eec3771-a3d2-4550-a24b-d10104f36fc7	upn	openid-connect	oidc-usermodel-property-mapper	\N	9a266253-af14-4478-8a65-33070b82df1a
18de3ea7-1332-4366-a04f-698fd7aa40f3	groups	openid-connect	oidc-usermodel-realm-role-mapper	\N	9a266253-af14-4478-8a65-33070b82df1a
82bcfab1-bbf0-403c-acea-f03de87997b1	role list	saml	saml-role-list-mapper	\N	05ef0639-aae9-4a1f-ad43-b8f6d1eb0a50
98a94df9-2082-4464-9919-37c845ce2a2d	full name	openid-connect	oidc-full-name-mapper	\N	30af825f-1ec9-45af-a678-279a6458f78d
e0e65e85-05a3-4902-a769-01131bf611ad	family name	openid-connect	oidc-usermodel-property-mapper	\N	30af825f-1ec9-45af-a678-279a6458f78d
0799d77a-e6f1-419e-97b2-88165dd357b6	given name	openid-connect	oidc-usermodel-property-mapper	\N	30af825f-1ec9-45af-a678-279a6458f78d
79d79ccd-fe3f-499d-a25a-9cac70756d91	middle name	openid-connect	oidc-usermodel-attribute-mapper	\N	30af825f-1ec9-45af-a678-279a6458f78d
2daa6d52-8cc4-4067-94b3-1bdd2cfdf290	nickname	openid-connect	oidc-usermodel-attribute-mapper	\N	30af825f-1ec9-45af-a678-279a6458f78d
bb9833a0-9e08-4102-8f35-f2b80b078633	username	openid-connect	oidc-usermodel-property-mapper	\N	30af825f-1ec9-45af-a678-279a6458f78d
a2667898-6e72-454c-a703-f9e6cb08cfae	profile	openid-connect	oidc-usermodel-attribute-mapper	\N	30af825f-1ec9-45af-a678-279a6458f78d
b5eed276-4f7b-4cbe-b37c-8d1dea67cdcb	picture	openid-connect	oidc-usermodel-attribute-mapper	\N	30af825f-1ec9-45af-a678-279a6458f78d
b3e31d79-b8a1-4d51-9967-1ee7e6b1e492	website	openid-connect	oidc-usermodel-attribute-mapper	\N	30af825f-1ec9-45af-a678-279a6458f78d
78816ee2-33d0-4390-83c5-fd5859920c69	gender	openid-connect	oidc-usermodel-attribute-mapper	\N	30af825f-1ec9-45af-a678-279a6458f78d
65acd8f3-07e2-4e4b-a7dd-eed5f0fd1813	birthdate	openid-connect	oidc-usermodel-attribute-mapper	\N	30af825f-1ec9-45af-a678-279a6458f78d
bac30b52-7f00-4691-9fa8-92df8bcbcf49	zoneinfo	openid-connect	oidc-usermodel-attribute-mapper	\N	30af825f-1ec9-45af-a678-279a6458f78d
42560a99-2290-4843-8fba-523dcdbd0788	locale	openid-connect	oidc-usermodel-attribute-mapper	\N	30af825f-1ec9-45af-a678-279a6458f78d
a0388ac8-15e7-4430-80e7-097c9f772664	updated at	openid-connect	oidc-usermodel-attribute-mapper	\N	30af825f-1ec9-45af-a678-279a6458f78d
cf9b4c4b-3bf1-416c-b1a9-08f6169dba1f	email	openid-connect	oidc-usermodel-property-mapper	\N	61b57419-d020-4777-a159-e3b13122842e
3ea7bfd1-2cf3-4eb1-a430-4cf3f2b6d644	email verified	openid-connect	oidc-usermodel-property-mapper	\N	61b57419-d020-4777-a159-e3b13122842e
8de91f50-442e-43cc-8342-ca49f40fee94	address	openid-connect	oidc-address-mapper	\N	880bb5d8-829b-4e64-b07d-913dab25091a
d80d3fb3-4500-4da5-93c8-f67a5d9da525	phone number	openid-connect	oidc-usermodel-attribute-mapper	\N	2d9c89c5-4764-48eb-9140-c34aceb2b1c7
367740d7-110b-4b64-aa6f-db78e3a15e6d	phone number verified	openid-connect	oidc-usermodel-attribute-mapper	\N	2d9c89c5-4764-48eb-9140-c34aceb2b1c7
9e216098-5604-4df3-9847-3bb452a740e5	realm roles	openid-connect	oidc-usermodel-realm-role-mapper	\N	88f3f2e7-c4fd-4439-a3a3-8fab2c64bd03
74c5d48a-1937-49c3-9c29-d6b3e9c3611e	client roles	openid-connect	oidc-usermodel-client-role-mapper	\N	88f3f2e7-c4fd-4439-a3a3-8fab2c64bd03
e94d0a2f-e28c-4e0a-91a7-bcca44e358b8	audience resolve	openid-connect	oidc-audience-resolve-mapper	\N	88f3f2e7-c4fd-4439-a3a3-8fab2c64bd03
57554b5a-f6af-486c-a809-9b738a93bc30	allowed web origins	openid-connect	oidc-allowed-origins-mapper	\N	e682941a-b6e0-4a6a-8539-150b36cb85e9
7446f5c9-83d7-431e-8fdd-182001b25c00	upn	openid-connect	oidc-usermodel-property-mapper	\N	b9cbe9c2-5da1-4be4-a1e5-c308e6b222b3
5a139d14-fcff-4c0d-b6ad-c15f5d299b21	groups	openid-connect	oidc-usermodel-realm-role-mapper	\N	b9cbe9c2-5da1-4be4-a1e5-c308e6b222b3
9f531e12-a61b-43e1-9327-f9bab9060437	locale	openid-connect	oidc-usermodel-attribute-mapper	d8945854-74b3-460b-9089-00fbc8c14672	\N
7fec4f29-e1c1-418f-b282-0c93048c629a	Client ID	openid-connect	oidc-usersessionmodel-note-mapper	aed69ab6-ca48-4b7f-a94d-629361b04bb8	\N
cf8dea8c-630c-44b0-950e-b0b25867a9dd	Client Host	openid-connect	oidc-usersessionmodel-note-mapper	aed69ab6-ca48-4b7f-a94d-629361b04bb8	\N
e326ef87-82b5-4155-9a77-3eea4bbeecbe	Client IP Address	openid-connect	oidc-usersessionmodel-note-mapper	aed69ab6-ca48-4b7f-a94d-629361b04bb8	\N
7ef5c825-254d-44a5-aad8-624facc2f26c	Username	openid-connect	oidc-usermodel-property-mapper	aed69ab6-ca48-4b7f-a94d-629361b04bb8	\N
\.


--
-- Data for Name: protocol_mapper_config; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.protocol_mapper_config (protocol_mapper_id, value, name) FROM stdin;
2281d397-4263-462b-bb83-7b7c5c7d50d6	true	userinfo.token.claim
2281d397-4263-462b-bb83-7b7c5c7d50d6	locale	user.attribute
2281d397-4263-462b-bb83-7b7c5c7d50d6	true	id.token.claim
2281d397-4263-462b-bb83-7b7c5c7d50d6	true	access.token.claim
2281d397-4263-462b-bb83-7b7c5c7d50d6	locale	claim.name
2281d397-4263-462b-bb83-7b7c5c7d50d6	String	jsonType.label
cd333b14-9da9-49d2-ac24-c4cf14c386fa	false	single
cd333b14-9da9-49d2-ac24-c4cf14c386fa	Basic	attribute.nameformat
cd333b14-9da9-49d2-ac24-c4cf14c386fa	Role	attribute.name
02954db9-69d2-4e36-b6f8-602534985614	true	userinfo.token.claim
02954db9-69d2-4e36-b6f8-602534985614	true	id.token.claim
02954db9-69d2-4e36-b6f8-602534985614	true	access.token.claim
1d7a0e05-8fad-4f38-a4ed-a634f0bf868b	true	userinfo.token.claim
1d7a0e05-8fad-4f38-a4ed-a634f0bf868b	lastName	user.attribute
1d7a0e05-8fad-4f38-a4ed-a634f0bf868b	true	id.token.claim
1d7a0e05-8fad-4f38-a4ed-a634f0bf868b	true	access.token.claim
1d7a0e05-8fad-4f38-a4ed-a634f0bf868b	family_name	claim.name
1d7a0e05-8fad-4f38-a4ed-a634f0bf868b	String	jsonType.label
3dee66f3-af8c-42aa-bcdb-c9c0a8a4a35b	true	userinfo.token.claim
3dee66f3-af8c-42aa-bcdb-c9c0a8a4a35b	firstName	user.attribute
3dee66f3-af8c-42aa-bcdb-c9c0a8a4a35b	true	id.token.claim
3dee66f3-af8c-42aa-bcdb-c9c0a8a4a35b	true	access.token.claim
3dee66f3-af8c-42aa-bcdb-c9c0a8a4a35b	given_name	claim.name
3dee66f3-af8c-42aa-bcdb-c9c0a8a4a35b	String	jsonType.label
d0ea70be-cddc-41c7-b95e-41f654f791d7	true	userinfo.token.claim
d0ea70be-cddc-41c7-b95e-41f654f791d7	middleName	user.attribute
d0ea70be-cddc-41c7-b95e-41f654f791d7	true	id.token.claim
d0ea70be-cddc-41c7-b95e-41f654f791d7	true	access.token.claim
d0ea70be-cddc-41c7-b95e-41f654f791d7	middle_name	claim.name
d0ea70be-cddc-41c7-b95e-41f654f791d7	String	jsonType.label
5d0571c0-5be2-4ca4-b311-ec4e30261848	true	userinfo.token.claim
5d0571c0-5be2-4ca4-b311-ec4e30261848	nickname	user.attribute
5d0571c0-5be2-4ca4-b311-ec4e30261848	true	id.token.claim
5d0571c0-5be2-4ca4-b311-ec4e30261848	true	access.token.claim
5d0571c0-5be2-4ca4-b311-ec4e30261848	nickname	claim.name
5d0571c0-5be2-4ca4-b311-ec4e30261848	String	jsonType.label
3045cad8-9230-424d-83a2-9c96ff6688dd	true	userinfo.token.claim
3045cad8-9230-424d-83a2-9c96ff6688dd	username	user.attribute
3045cad8-9230-424d-83a2-9c96ff6688dd	true	id.token.claim
3045cad8-9230-424d-83a2-9c96ff6688dd	true	access.token.claim
3045cad8-9230-424d-83a2-9c96ff6688dd	preferred_username	claim.name
3045cad8-9230-424d-83a2-9c96ff6688dd	String	jsonType.label
821e7296-f3a3-4c9c-b29b-30e3765272e8	true	userinfo.token.claim
821e7296-f3a3-4c9c-b29b-30e3765272e8	profile	user.attribute
821e7296-f3a3-4c9c-b29b-30e3765272e8	true	id.token.claim
821e7296-f3a3-4c9c-b29b-30e3765272e8	true	access.token.claim
821e7296-f3a3-4c9c-b29b-30e3765272e8	profile	claim.name
821e7296-f3a3-4c9c-b29b-30e3765272e8	String	jsonType.label
d1949eb1-1a52-42e7-93a7-f74ad96577dc	true	userinfo.token.claim
d1949eb1-1a52-42e7-93a7-f74ad96577dc	picture	user.attribute
d1949eb1-1a52-42e7-93a7-f74ad96577dc	true	id.token.claim
d1949eb1-1a52-42e7-93a7-f74ad96577dc	true	access.token.claim
d1949eb1-1a52-42e7-93a7-f74ad96577dc	picture	claim.name
d1949eb1-1a52-42e7-93a7-f74ad96577dc	String	jsonType.label
2d936bdd-c084-4221-9a91-32c71d3996d4	true	userinfo.token.claim
2d936bdd-c084-4221-9a91-32c71d3996d4	website	user.attribute
2d936bdd-c084-4221-9a91-32c71d3996d4	true	id.token.claim
2d936bdd-c084-4221-9a91-32c71d3996d4	true	access.token.claim
2d936bdd-c084-4221-9a91-32c71d3996d4	website	claim.name
2d936bdd-c084-4221-9a91-32c71d3996d4	String	jsonType.label
91b6ef24-01bf-471b-89c7-55d86e4a682f	true	userinfo.token.claim
91b6ef24-01bf-471b-89c7-55d86e4a682f	gender	user.attribute
91b6ef24-01bf-471b-89c7-55d86e4a682f	true	id.token.claim
91b6ef24-01bf-471b-89c7-55d86e4a682f	true	access.token.claim
91b6ef24-01bf-471b-89c7-55d86e4a682f	gender	claim.name
91b6ef24-01bf-471b-89c7-55d86e4a682f	String	jsonType.label
26d600f6-93a9-4556-aab4-b49feafef2da	true	userinfo.token.claim
26d600f6-93a9-4556-aab4-b49feafef2da	birthdate	user.attribute
26d600f6-93a9-4556-aab4-b49feafef2da	true	id.token.claim
26d600f6-93a9-4556-aab4-b49feafef2da	true	access.token.claim
26d600f6-93a9-4556-aab4-b49feafef2da	birthdate	claim.name
26d600f6-93a9-4556-aab4-b49feafef2da	String	jsonType.label
84512d85-5c7d-452e-92b6-08583b95a10a	true	userinfo.token.claim
84512d85-5c7d-452e-92b6-08583b95a10a	zoneinfo	user.attribute
84512d85-5c7d-452e-92b6-08583b95a10a	true	id.token.claim
84512d85-5c7d-452e-92b6-08583b95a10a	true	access.token.claim
84512d85-5c7d-452e-92b6-08583b95a10a	zoneinfo	claim.name
84512d85-5c7d-452e-92b6-08583b95a10a	String	jsonType.label
520727d3-23e8-44bf-8151-8c1a88c66278	true	userinfo.token.claim
520727d3-23e8-44bf-8151-8c1a88c66278	locale	user.attribute
520727d3-23e8-44bf-8151-8c1a88c66278	true	id.token.claim
520727d3-23e8-44bf-8151-8c1a88c66278	true	access.token.claim
520727d3-23e8-44bf-8151-8c1a88c66278	locale	claim.name
520727d3-23e8-44bf-8151-8c1a88c66278	String	jsonType.label
158a0943-9e13-480f-8e0b-95342d96d12a	true	userinfo.token.claim
158a0943-9e13-480f-8e0b-95342d96d12a	updatedAt	user.attribute
158a0943-9e13-480f-8e0b-95342d96d12a	true	id.token.claim
158a0943-9e13-480f-8e0b-95342d96d12a	true	access.token.claim
158a0943-9e13-480f-8e0b-95342d96d12a	updated_at	claim.name
158a0943-9e13-480f-8e0b-95342d96d12a	String	jsonType.label
203edf52-38f8-49fc-8810-a46cb2bdc7c4	true	userinfo.token.claim
203edf52-38f8-49fc-8810-a46cb2bdc7c4	email	user.attribute
203edf52-38f8-49fc-8810-a46cb2bdc7c4	true	id.token.claim
203edf52-38f8-49fc-8810-a46cb2bdc7c4	true	access.token.claim
203edf52-38f8-49fc-8810-a46cb2bdc7c4	email	claim.name
203edf52-38f8-49fc-8810-a46cb2bdc7c4	String	jsonType.label
7b959da5-d4de-4e89-bf5c-c3bad0f1248e	true	userinfo.token.claim
7b959da5-d4de-4e89-bf5c-c3bad0f1248e	emailVerified	user.attribute
7b959da5-d4de-4e89-bf5c-c3bad0f1248e	true	id.token.claim
7b959da5-d4de-4e89-bf5c-c3bad0f1248e	true	access.token.claim
7b959da5-d4de-4e89-bf5c-c3bad0f1248e	email_verified	claim.name
7b959da5-d4de-4e89-bf5c-c3bad0f1248e	boolean	jsonType.label
14850045-d90f-4758-a71a-ea32373411ba	formatted	user.attribute.formatted
14850045-d90f-4758-a71a-ea32373411ba	country	user.attribute.country
14850045-d90f-4758-a71a-ea32373411ba	postal_code	user.attribute.postal_code
14850045-d90f-4758-a71a-ea32373411ba	true	userinfo.token.claim
14850045-d90f-4758-a71a-ea32373411ba	street	user.attribute.street
14850045-d90f-4758-a71a-ea32373411ba	true	id.token.claim
14850045-d90f-4758-a71a-ea32373411ba	region	user.attribute.region
14850045-d90f-4758-a71a-ea32373411ba	true	access.token.claim
14850045-d90f-4758-a71a-ea32373411ba	locality	user.attribute.locality
178eb17c-fc50-4efb-a1b7-e860da413a06	true	userinfo.token.claim
178eb17c-fc50-4efb-a1b7-e860da413a06	phoneNumber	user.attribute
178eb17c-fc50-4efb-a1b7-e860da413a06	true	id.token.claim
178eb17c-fc50-4efb-a1b7-e860da413a06	true	access.token.claim
178eb17c-fc50-4efb-a1b7-e860da413a06	phone_number	claim.name
178eb17c-fc50-4efb-a1b7-e860da413a06	String	jsonType.label
bf5faaac-26da-4238-9d7c-8166faffe683	true	userinfo.token.claim
bf5faaac-26da-4238-9d7c-8166faffe683	phoneNumberVerified	user.attribute
bf5faaac-26da-4238-9d7c-8166faffe683	true	id.token.claim
bf5faaac-26da-4238-9d7c-8166faffe683	true	access.token.claim
bf5faaac-26da-4238-9d7c-8166faffe683	phone_number_verified	claim.name
bf5faaac-26da-4238-9d7c-8166faffe683	boolean	jsonType.label
05e98394-df87-4448-b3ce-27a9b31d76df	true	multivalued
05e98394-df87-4448-b3ce-27a9b31d76df	foo	user.attribute
05e98394-df87-4448-b3ce-27a9b31d76df	true	access.token.claim
05e98394-df87-4448-b3ce-27a9b31d76df	realm_access.roles	claim.name
05e98394-df87-4448-b3ce-27a9b31d76df	String	jsonType.label
939cd1ba-6df6-4624-b8ce-59357bc133ba	true	multivalued
939cd1ba-6df6-4624-b8ce-59357bc133ba	foo	user.attribute
939cd1ba-6df6-4624-b8ce-59357bc133ba	true	access.token.claim
939cd1ba-6df6-4624-b8ce-59357bc133ba	resource_access.${client_id}.roles	claim.name
939cd1ba-6df6-4624-b8ce-59357bc133ba	String	jsonType.label
4eec3771-a3d2-4550-a24b-d10104f36fc7	true	userinfo.token.claim
4eec3771-a3d2-4550-a24b-d10104f36fc7	username	user.attribute
4eec3771-a3d2-4550-a24b-d10104f36fc7	true	id.token.claim
4eec3771-a3d2-4550-a24b-d10104f36fc7	true	access.token.claim
4eec3771-a3d2-4550-a24b-d10104f36fc7	upn	claim.name
4eec3771-a3d2-4550-a24b-d10104f36fc7	String	jsonType.label
18de3ea7-1332-4366-a04f-698fd7aa40f3	true	multivalued
18de3ea7-1332-4366-a04f-698fd7aa40f3	foo	user.attribute
18de3ea7-1332-4366-a04f-698fd7aa40f3	true	id.token.claim
18de3ea7-1332-4366-a04f-698fd7aa40f3	true	access.token.claim
18de3ea7-1332-4366-a04f-698fd7aa40f3	groups	claim.name
18de3ea7-1332-4366-a04f-698fd7aa40f3	String	jsonType.label
82bcfab1-bbf0-403c-acea-f03de87997b1	false	single
82bcfab1-bbf0-403c-acea-f03de87997b1	Basic	attribute.nameformat
82bcfab1-bbf0-403c-acea-f03de87997b1	Role	attribute.name
98a94df9-2082-4464-9919-37c845ce2a2d	true	userinfo.token.claim
98a94df9-2082-4464-9919-37c845ce2a2d	true	id.token.claim
98a94df9-2082-4464-9919-37c845ce2a2d	true	access.token.claim
e0e65e85-05a3-4902-a769-01131bf611ad	true	userinfo.token.claim
e0e65e85-05a3-4902-a769-01131bf611ad	lastName	user.attribute
e0e65e85-05a3-4902-a769-01131bf611ad	true	id.token.claim
e0e65e85-05a3-4902-a769-01131bf611ad	true	access.token.claim
e0e65e85-05a3-4902-a769-01131bf611ad	family_name	claim.name
e0e65e85-05a3-4902-a769-01131bf611ad	String	jsonType.label
0799d77a-e6f1-419e-97b2-88165dd357b6	true	userinfo.token.claim
0799d77a-e6f1-419e-97b2-88165dd357b6	firstName	user.attribute
0799d77a-e6f1-419e-97b2-88165dd357b6	true	id.token.claim
0799d77a-e6f1-419e-97b2-88165dd357b6	true	access.token.claim
0799d77a-e6f1-419e-97b2-88165dd357b6	given_name	claim.name
0799d77a-e6f1-419e-97b2-88165dd357b6	String	jsonType.label
79d79ccd-fe3f-499d-a25a-9cac70756d91	true	userinfo.token.claim
79d79ccd-fe3f-499d-a25a-9cac70756d91	middleName	user.attribute
79d79ccd-fe3f-499d-a25a-9cac70756d91	true	id.token.claim
79d79ccd-fe3f-499d-a25a-9cac70756d91	true	access.token.claim
79d79ccd-fe3f-499d-a25a-9cac70756d91	middle_name	claim.name
79d79ccd-fe3f-499d-a25a-9cac70756d91	String	jsonType.label
2daa6d52-8cc4-4067-94b3-1bdd2cfdf290	true	userinfo.token.claim
2daa6d52-8cc4-4067-94b3-1bdd2cfdf290	nickname	user.attribute
2daa6d52-8cc4-4067-94b3-1bdd2cfdf290	true	id.token.claim
2daa6d52-8cc4-4067-94b3-1bdd2cfdf290	true	access.token.claim
2daa6d52-8cc4-4067-94b3-1bdd2cfdf290	nickname	claim.name
2daa6d52-8cc4-4067-94b3-1bdd2cfdf290	String	jsonType.label
bb9833a0-9e08-4102-8f35-f2b80b078633	true	userinfo.token.claim
bb9833a0-9e08-4102-8f35-f2b80b078633	username	user.attribute
bb9833a0-9e08-4102-8f35-f2b80b078633	true	id.token.claim
bb9833a0-9e08-4102-8f35-f2b80b078633	true	access.token.claim
bb9833a0-9e08-4102-8f35-f2b80b078633	preferred_username	claim.name
bb9833a0-9e08-4102-8f35-f2b80b078633	String	jsonType.label
a2667898-6e72-454c-a703-f9e6cb08cfae	true	userinfo.token.claim
a2667898-6e72-454c-a703-f9e6cb08cfae	profile	user.attribute
a2667898-6e72-454c-a703-f9e6cb08cfae	true	id.token.claim
a2667898-6e72-454c-a703-f9e6cb08cfae	true	access.token.claim
a2667898-6e72-454c-a703-f9e6cb08cfae	profile	claim.name
a2667898-6e72-454c-a703-f9e6cb08cfae	String	jsonType.label
b5eed276-4f7b-4cbe-b37c-8d1dea67cdcb	true	userinfo.token.claim
b5eed276-4f7b-4cbe-b37c-8d1dea67cdcb	picture	user.attribute
b5eed276-4f7b-4cbe-b37c-8d1dea67cdcb	true	id.token.claim
b5eed276-4f7b-4cbe-b37c-8d1dea67cdcb	true	access.token.claim
b5eed276-4f7b-4cbe-b37c-8d1dea67cdcb	picture	claim.name
b5eed276-4f7b-4cbe-b37c-8d1dea67cdcb	String	jsonType.label
b3e31d79-b8a1-4d51-9967-1ee7e6b1e492	true	userinfo.token.claim
b3e31d79-b8a1-4d51-9967-1ee7e6b1e492	website	user.attribute
b3e31d79-b8a1-4d51-9967-1ee7e6b1e492	true	id.token.claim
b3e31d79-b8a1-4d51-9967-1ee7e6b1e492	true	access.token.claim
b3e31d79-b8a1-4d51-9967-1ee7e6b1e492	website	claim.name
b3e31d79-b8a1-4d51-9967-1ee7e6b1e492	String	jsonType.label
78816ee2-33d0-4390-83c5-fd5859920c69	true	userinfo.token.claim
78816ee2-33d0-4390-83c5-fd5859920c69	gender	user.attribute
78816ee2-33d0-4390-83c5-fd5859920c69	true	id.token.claim
78816ee2-33d0-4390-83c5-fd5859920c69	true	access.token.claim
78816ee2-33d0-4390-83c5-fd5859920c69	gender	claim.name
78816ee2-33d0-4390-83c5-fd5859920c69	String	jsonType.label
65acd8f3-07e2-4e4b-a7dd-eed5f0fd1813	true	userinfo.token.claim
65acd8f3-07e2-4e4b-a7dd-eed5f0fd1813	birthdate	user.attribute
65acd8f3-07e2-4e4b-a7dd-eed5f0fd1813	true	id.token.claim
65acd8f3-07e2-4e4b-a7dd-eed5f0fd1813	true	access.token.claim
65acd8f3-07e2-4e4b-a7dd-eed5f0fd1813	birthdate	claim.name
65acd8f3-07e2-4e4b-a7dd-eed5f0fd1813	String	jsonType.label
bac30b52-7f00-4691-9fa8-92df8bcbcf49	true	userinfo.token.claim
bac30b52-7f00-4691-9fa8-92df8bcbcf49	zoneinfo	user.attribute
bac30b52-7f00-4691-9fa8-92df8bcbcf49	true	id.token.claim
bac30b52-7f00-4691-9fa8-92df8bcbcf49	true	access.token.claim
bac30b52-7f00-4691-9fa8-92df8bcbcf49	zoneinfo	claim.name
bac30b52-7f00-4691-9fa8-92df8bcbcf49	String	jsonType.label
42560a99-2290-4843-8fba-523dcdbd0788	true	userinfo.token.claim
42560a99-2290-4843-8fba-523dcdbd0788	locale	user.attribute
42560a99-2290-4843-8fba-523dcdbd0788	true	id.token.claim
42560a99-2290-4843-8fba-523dcdbd0788	true	access.token.claim
42560a99-2290-4843-8fba-523dcdbd0788	locale	claim.name
42560a99-2290-4843-8fba-523dcdbd0788	String	jsonType.label
a0388ac8-15e7-4430-80e7-097c9f772664	true	userinfo.token.claim
a0388ac8-15e7-4430-80e7-097c9f772664	updatedAt	user.attribute
a0388ac8-15e7-4430-80e7-097c9f772664	true	id.token.claim
a0388ac8-15e7-4430-80e7-097c9f772664	true	access.token.claim
a0388ac8-15e7-4430-80e7-097c9f772664	updated_at	claim.name
a0388ac8-15e7-4430-80e7-097c9f772664	String	jsonType.label
cf9b4c4b-3bf1-416c-b1a9-08f6169dba1f	true	userinfo.token.claim
cf9b4c4b-3bf1-416c-b1a9-08f6169dba1f	email	user.attribute
cf9b4c4b-3bf1-416c-b1a9-08f6169dba1f	true	id.token.claim
cf9b4c4b-3bf1-416c-b1a9-08f6169dba1f	true	access.token.claim
cf9b4c4b-3bf1-416c-b1a9-08f6169dba1f	email	claim.name
cf9b4c4b-3bf1-416c-b1a9-08f6169dba1f	String	jsonType.label
3ea7bfd1-2cf3-4eb1-a430-4cf3f2b6d644	true	userinfo.token.claim
3ea7bfd1-2cf3-4eb1-a430-4cf3f2b6d644	emailVerified	user.attribute
3ea7bfd1-2cf3-4eb1-a430-4cf3f2b6d644	true	id.token.claim
3ea7bfd1-2cf3-4eb1-a430-4cf3f2b6d644	true	access.token.claim
3ea7bfd1-2cf3-4eb1-a430-4cf3f2b6d644	email_verified	claim.name
3ea7bfd1-2cf3-4eb1-a430-4cf3f2b6d644	boolean	jsonType.label
8de91f50-442e-43cc-8342-ca49f40fee94	formatted	user.attribute.formatted
8de91f50-442e-43cc-8342-ca49f40fee94	country	user.attribute.country
8de91f50-442e-43cc-8342-ca49f40fee94	postal_code	user.attribute.postal_code
8de91f50-442e-43cc-8342-ca49f40fee94	true	userinfo.token.claim
8de91f50-442e-43cc-8342-ca49f40fee94	street	user.attribute.street
8de91f50-442e-43cc-8342-ca49f40fee94	true	id.token.claim
8de91f50-442e-43cc-8342-ca49f40fee94	region	user.attribute.region
8de91f50-442e-43cc-8342-ca49f40fee94	true	access.token.claim
8de91f50-442e-43cc-8342-ca49f40fee94	locality	user.attribute.locality
d80d3fb3-4500-4da5-93c8-f67a5d9da525	true	userinfo.token.claim
d80d3fb3-4500-4da5-93c8-f67a5d9da525	phoneNumber	user.attribute
d80d3fb3-4500-4da5-93c8-f67a5d9da525	true	id.token.claim
d80d3fb3-4500-4da5-93c8-f67a5d9da525	true	access.token.claim
d80d3fb3-4500-4da5-93c8-f67a5d9da525	phone_number	claim.name
d80d3fb3-4500-4da5-93c8-f67a5d9da525	String	jsonType.label
367740d7-110b-4b64-aa6f-db78e3a15e6d	true	userinfo.token.claim
367740d7-110b-4b64-aa6f-db78e3a15e6d	phoneNumberVerified	user.attribute
367740d7-110b-4b64-aa6f-db78e3a15e6d	true	id.token.claim
367740d7-110b-4b64-aa6f-db78e3a15e6d	true	access.token.claim
367740d7-110b-4b64-aa6f-db78e3a15e6d	phone_number_verified	claim.name
367740d7-110b-4b64-aa6f-db78e3a15e6d	boolean	jsonType.label
9e216098-5604-4df3-9847-3bb452a740e5	true	multivalued
9e216098-5604-4df3-9847-3bb452a740e5	foo	user.attribute
9e216098-5604-4df3-9847-3bb452a740e5	true	access.token.claim
9e216098-5604-4df3-9847-3bb452a740e5	realm_access.roles	claim.name
9e216098-5604-4df3-9847-3bb452a740e5	String	jsonType.label
74c5d48a-1937-49c3-9c29-d6b3e9c3611e	true	multivalued
74c5d48a-1937-49c3-9c29-d6b3e9c3611e	foo	user.attribute
74c5d48a-1937-49c3-9c29-d6b3e9c3611e	true	access.token.claim
74c5d48a-1937-49c3-9c29-d6b3e9c3611e	resource_access.${client_id}.roles	claim.name
74c5d48a-1937-49c3-9c29-d6b3e9c3611e	String	jsonType.label
7446f5c9-83d7-431e-8fdd-182001b25c00	true	userinfo.token.claim
7446f5c9-83d7-431e-8fdd-182001b25c00	username	user.attribute
7446f5c9-83d7-431e-8fdd-182001b25c00	true	id.token.claim
7446f5c9-83d7-431e-8fdd-182001b25c00	true	access.token.claim
7446f5c9-83d7-431e-8fdd-182001b25c00	upn	claim.name
7446f5c9-83d7-431e-8fdd-182001b25c00	String	jsonType.label
5a139d14-fcff-4c0d-b6ad-c15f5d299b21	true	multivalued
5a139d14-fcff-4c0d-b6ad-c15f5d299b21	foo	user.attribute
5a139d14-fcff-4c0d-b6ad-c15f5d299b21	true	id.token.claim
5a139d14-fcff-4c0d-b6ad-c15f5d299b21	true	access.token.claim
5a139d14-fcff-4c0d-b6ad-c15f5d299b21	groups	claim.name
5a139d14-fcff-4c0d-b6ad-c15f5d299b21	String	jsonType.label
9f531e12-a61b-43e1-9327-f9bab9060437	true	userinfo.token.claim
9f531e12-a61b-43e1-9327-f9bab9060437	locale	user.attribute
9f531e12-a61b-43e1-9327-f9bab9060437	true	id.token.claim
9f531e12-a61b-43e1-9327-f9bab9060437	true	access.token.claim
9f531e12-a61b-43e1-9327-f9bab9060437	locale	claim.name
9f531e12-a61b-43e1-9327-f9bab9060437	String	jsonType.label
7fec4f29-e1c1-418f-b282-0c93048c629a	clientId	user.session.note
7fec4f29-e1c1-418f-b282-0c93048c629a	true	id.token.claim
7fec4f29-e1c1-418f-b282-0c93048c629a	true	access.token.claim
7fec4f29-e1c1-418f-b282-0c93048c629a	clientId	claim.name
7fec4f29-e1c1-418f-b282-0c93048c629a	String	jsonType.label
cf8dea8c-630c-44b0-950e-b0b25867a9dd	clientHost	user.session.note
cf8dea8c-630c-44b0-950e-b0b25867a9dd	true	id.token.claim
cf8dea8c-630c-44b0-950e-b0b25867a9dd	true	access.token.claim
cf8dea8c-630c-44b0-950e-b0b25867a9dd	clientHost	claim.name
cf8dea8c-630c-44b0-950e-b0b25867a9dd	String	jsonType.label
e326ef87-82b5-4155-9a77-3eea4bbeecbe	clientAddress	user.session.note
e326ef87-82b5-4155-9a77-3eea4bbeecbe	true	id.token.claim
e326ef87-82b5-4155-9a77-3eea4bbeecbe	true	access.token.claim
e326ef87-82b5-4155-9a77-3eea4bbeecbe	clientAddress	claim.name
e326ef87-82b5-4155-9a77-3eea4bbeecbe	String	jsonType.label
7ef5c825-254d-44a5-aad8-624facc2f26c	true	userinfo.token.claim
7ef5c825-254d-44a5-aad8-624facc2f26c	username	user.attribute
7ef5c825-254d-44a5-aad8-624facc2f26c	true	id.token.claim
7ef5c825-254d-44a5-aad8-624facc2f26c	true	access.token.claim
7ef5c825-254d-44a5-aad8-624facc2f26c	user_name	claim.name
7ef5c825-254d-44a5-aad8-624facc2f26c	String	jsonType.label
\.


--
-- Data for Name: realm; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.realm (id, access_code_lifespan, user_action_lifespan, access_token_lifespan, account_theme, admin_theme, email_theme, enabled, events_enabled, events_expiration, login_theme, name, not_before, password_policy, registration_allowed, remember_me, reset_password_allowed, social, ssl_required, sso_idle_timeout, sso_max_lifespan, update_profile_on_soc_login, verify_email, master_admin_client, login_lifespan, internationalization_enabled, default_locale, reg_email_as_username, admin_events_enabled, admin_events_details_enabled, edit_username_allowed, otp_policy_counter, otp_policy_window, otp_policy_period, otp_policy_digits, otp_policy_alg, otp_policy_type, browser_flow, registration_flow, direct_grant_flow, reset_credentials_flow, client_auth_flow, offline_session_idle_timeout, revoke_refresh_token, access_token_life_implicit, login_with_email_allowed, duplicate_emails_allowed, docker_auth_flow, refresh_token_max_reuse, allow_user_managed_access, sso_max_lifespan_remember_me, sso_idle_timeout_remember_me) FROM stdin;
master	60	300	60	\N	\N	\N	t	f	0	\N	master	0	\N	f	f	f	f	EXTERNAL	1800	36000	f	f	6e4abf2f-cad7-4035-8014-f7b86990dee7	1800	f	\N	f	f	f	f	0	1	30	6	HmacSHA1	totp	95b9a29d-b501-4615-8c9b-84968b2e8a03	eccd353b-9457-4eb0-b750-0917d1157ddd	b66f38b7-9cc5-42c9-a721-c0b33578b7ce	3545f14e-f248-4ec2-93d8-da885e104de0	e769c1d6-210e-4885-bda1-2f8860c06cc9	2592000	f	900	t	f	dfea654f-225b-4da7-a0e3-de19cc7eb478	0	f	0	0
ModMappings	60	300	300	\N	\N	\N	t	f	0	\N	ModMappings	0	\N	f	f	f	f	EXTERNAL	1800	36000	f	f	b1781787-3740-49f7-9015-51485579c1e5	1800	f	\N	f	f	f	f	0	1	30	6	HmacSHA1	totp	a04d0d12-d12f-4fc3-956b-94a95cf4d377	05b34418-8d4f-4bf7-8e6e-e34cd3e2dc45	110d53fe-16d9-4d94-8f43-f11598617f73	f6c01ce9-afac-4a85-adf5-3e67f0bd0188	15269a41-3f30-4bbb-9491-3107bf2ec42b	2592000	f	900	t	f	7dda0288-0e27-431e-ba8c-5e66a411a92f	0	f	0	0
\.


--
-- Data for Name: realm_attribute; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.realm_attribute (name, value, realm_id) FROM stdin;
_browser_header.contentSecurityPolicyReportOnly		master
_browser_header.xContentTypeOptions	nosniff	master
_browser_header.xRobotsTag	none	master
_browser_header.xFrameOptions	SAMEORIGIN	master
_browser_header.contentSecurityPolicy	frame-src 'self'; frame-ancestors 'self'; object-src 'none';	master
_browser_header.xXSSProtection	1; mode=block	master
_browser_header.strictTransportSecurity	max-age=31536000; includeSubDomains	master
bruteForceProtected	false	master
permanentLockout	false	master
maxFailureWaitSeconds	900	master
minimumQuickLoginWaitSeconds	60	master
waitIncrementSeconds	60	master
quickLoginCheckMilliSeconds	1000	master
maxDeltaTimeSeconds	43200	master
failureFactor	30	master
displayName	Keycloak	master
displayNameHtml	<div class="kc-logo-text"><span>Keycloak</span></div>	master
offlineSessionMaxLifespanEnabled	false	master
offlineSessionMaxLifespan	5184000	master
_browser_header.contentSecurityPolicyReportOnly		ModMappings
_browser_header.xContentTypeOptions	nosniff	ModMappings
_browser_header.xRobotsTag	none	ModMappings
_browser_header.xFrameOptions	SAMEORIGIN	ModMappings
_browser_header.contentSecurityPolicy	frame-src 'self'; frame-ancestors 'self'; object-src 'none';	ModMappings
_browser_header.xXSSProtection	1; mode=block	ModMappings
_browser_header.strictTransportSecurity	max-age=31536000; includeSubDomains	ModMappings
bruteForceProtected	false	ModMappings
permanentLockout	false	ModMappings
maxFailureWaitSeconds	900	ModMappings
minimumQuickLoginWaitSeconds	60	ModMappings
waitIncrementSeconds	60	ModMappings
quickLoginCheckMilliSeconds	1000	ModMappings
maxDeltaTimeSeconds	43200	ModMappings
failureFactor	30	ModMappings
offlineSessionMaxLifespanEnabled	false	ModMappings
offlineSessionMaxLifespan	5184000	ModMappings
actionTokenGeneratedByAdminLifespan	43200	ModMappings
actionTokenGeneratedByUserLifespan	300	ModMappings
webAuthnPolicyRpEntityName	keycloak	ModMappings
webAuthnPolicySignatureAlgorithms	ES256	ModMappings
webAuthnPolicyRpId		ModMappings
webAuthnPolicyAttestationConveyancePreference	not specified	ModMappings
webAuthnPolicyAuthenticatorAttachment	not specified	ModMappings
webAuthnPolicyRequireResidentKey	not specified	ModMappings
webAuthnPolicyUserVerificationRequirement	not specified	ModMappings
webAuthnPolicyCreateTimeout	0	ModMappings
webAuthnPolicyAvoidSameAuthenticatorRegister	false	ModMappings
displayName	ModMappings API	ModMappings
\.


--
-- Data for Name: realm_default_groups; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.realm_default_groups (realm_id, group_id) FROM stdin;
\.


--
-- Data for Name: realm_default_roles; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.realm_default_roles (realm_id, role_id) FROM stdin;
master	5726c14b-12e1-4acf-ab64-a55d60b8c582
master	06b6040e-2d97-4054-92f6-aab17984a8fe
ModMappings	770e4d48-d5a7-4795-8818-af03c8cf2d14
ModMappings	c82225bf-240c-444a-b715-57bb9229718f
\.


--
-- Data for Name: realm_enabled_event_types; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.realm_enabled_event_types (realm_id, value) FROM stdin;
\.


--
-- Data for Name: realm_events_listeners; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.realm_events_listeners (realm_id, value) FROM stdin;
master	jboss-logging
ModMappings	jboss-logging
\.


--
-- Data for Name: realm_required_credential; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.realm_required_credential (type, form_label, input, secret, realm_id) FROM stdin;
password	password	t	t	master
password	password	t	t	ModMappings
\.


--
-- Data for Name: realm_smtp_config; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.realm_smtp_config (realm_id, value, name) FROM stdin;
\.


--
-- Data for Name: realm_supported_locales; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.realm_supported_locales (realm_id, value) FROM stdin;
\.


--
-- Data for Name: redirect_uris; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.redirect_uris (client_id, value) FROM stdin;
c5f82688-a1d0-4d07-96f3-6dd303a5bd61	/realms/master/account/*
24463fff-1058-4fc9-8c1e-9ecbcf7fa2e9	/admin/master/console/*
b7f0ad00-37c4-4e69-a0a9-ce0920c034c3	/realms/ModMappings/account/*
d8945854-74b3-460b-9089-00fbc8c14672	/admin/ModMappings/console/*
aed69ab6-ca48-4b7f-a94d-629361b04bb8	https://api.modmappings.org/*
\.


--
-- Data for Name: required_action_config; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.required_action_config (required_action_id, value, name) FROM stdin;
\.


--
-- Data for Name: required_action_provider; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.required_action_provider (id, alias, name, realm_id, enabled, default_action, provider_id, priority) FROM stdin;
36b4f722-4893-4b79-9181-d8026c8dbbfb	VERIFY_EMAIL	Verify Email	master	t	f	VERIFY_EMAIL	50
8e05809c-934b-437e-a86a-2ac1e4819836	UPDATE_PROFILE	Update Profile	master	t	f	UPDATE_PROFILE	40
1a55f9d3-b1ee-4e7f-aee5-2dad2ab82509	CONFIGURE_TOTP	Configure OTP	master	t	f	CONFIGURE_TOTP	10
75caef10-75b8-4c9c-82bb-158466d938f6	UPDATE_PASSWORD	Update Password	master	t	f	UPDATE_PASSWORD	30
d7574e48-ab02-4051-a195-959fee55d450	terms_and_conditions	Terms and Conditions	master	f	f	terms_and_conditions	20
43e6eb4f-c5f5-42fd-9f7d-293b2ed76061	VERIFY_EMAIL	Verify Email	ModMappings	t	f	VERIFY_EMAIL	50
e1411971-b178-4de1-a118-89edde2e2f80	UPDATE_PROFILE	Update Profile	ModMappings	t	f	UPDATE_PROFILE	40
75ebe615-f8c4-41ac-b261-4e912d229903	CONFIGURE_TOTP	Configure OTP	ModMappings	t	f	CONFIGURE_TOTP	10
4066598a-a669-4a9b-b348-6ccd96389c8a	UPDATE_PASSWORD	Update Password	ModMappings	t	f	UPDATE_PASSWORD	30
4958a13c-673b-4207-97e6-7ca493fe5316	terms_and_conditions	Terms and Conditions	ModMappings	f	f	terms_and_conditions	20
\.


--
-- Data for Name: resource_attribute; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.resource_attribute (id, name, value, resource_id) FROM stdin;
\.


--
-- Data for Name: resource_policy; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.resource_policy (resource_id, policy_id) FROM stdin;
\.


--
-- Data for Name: resource_scope; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.resource_scope (resource_id, scope_id) FROM stdin;
\.


--
-- Data for Name: resource_server; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.resource_server (id, allow_rs_remote_mgmt, policy_enforce_mode, decision_strategy) FROM stdin;
aed69ab6-ca48-4b7f-a94d-629361b04bb8	t	0	1
\.


--
-- Data for Name: resource_server_perm_ticket; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.resource_server_perm_ticket (id, owner, requester, created_timestamp, granted_timestamp, resource_id, scope_id, resource_server_id, policy_id) FROM stdin;
\.


--
-- Data for Name: resource_server_policy; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.resource_server_policy (id, name, description, type, decision_strategy, logic, resource_server_id, owner) FROM stdin;
22d389b9-62a8-481e-87be-7d38f901a574	Default Policy	A policy that grants access only for users within this realm	js	0	0	aed69ab6-ca48-4b7f-a94d-629361b04bb8	\N
2fa44c63-4ea8-4438-a6fa-2b7f4306a307	Default Permission	A permission that applies to the default resource type	resource	1	0	aed69ab6-ca48-4b7f-a94d-629361b04bb8	\N
\.


--
-- Data for Name: resource_server_resource; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.resource_server_resource (id, name, type, icon_uri, owner, resource_server_id, owner_managed_access, display_name) FROM stdin;
21a192a1-0ad0-4510-9e16-f5a812027d23	Default Resource	urn:api:resources:default	\N	aed69ab6-ca48-4b7f-a94d-629361b04bb8	aed69ab6-ca48-4b7f-a94d-629361b04bb8	f	\N
\.


--
-- Data for Name: resource_server_scope; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.resource_server_scope (id, name, icon_uri, resource_server_id, display_name) FROM stdin;
\.


--
-- Data for Name: resource_uris; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.resource_uris (resource_id, value) FROM stdin;
21a192a1-0ad0-4510-9e16-f5a812027d23	/*
\.


--
-- Data for Name: role_attribute; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.role_attribute (id, role_id, name, value) FROM stdin;
\.


--
-- Data for Name: scope_mapping; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.scope_mapping (client_id, role_id) FROM stdin;
\.


--
-- Data for Name: scope_policy; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.scope_policy (scope_id, policy_id) FROM stdin;
\.


--
-- Data for Name: user_attribute; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.user_attribute (name, value, user_id, id) FROM stdin;
\.


--
-- Data for Name: user_consent; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.user_consent (id, client_id, user_id, created_date, last_updated_date, client_storage_provider, external_client_id) FROM stdin;
\.


--
-- Data for Name: user_consent_client_scope; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.user_consent_client_scope (user_consent_id, scope_id) FROM stdin;
\.


--
-- Data for Name: user_entity; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.user_entity (id, email, email_constraint, email_verified, enabled, federation_link, first_name, last_name, realm_id, username, created_timestamp, service_account_client_link, not_before) FROM stdin;
5cb3ca77-47ab-4721-a1ad-7b1868559aa5	\N	2d944765-debc-48ea-b11e-3f59092bc912	f	t	\N	\N	\N	master	admin	1579205750747	\N	0
9df4d504-9378-433e-b4e9-3e5a6600f0f1	\N	48ac4d15-57ff-4827-84a1-8257b9d1a247	f	t	\N	\N	\N	ModMappings	service-account-api	1579206009912	aed69ab6-ca48-4b7f-a94d-629361b04bb8	0
5c150731-d9e2-41c0-8091-836e67699cf1	bob@modmappings.com	bob@modmappings.com	t	t	\N	Bob	Password: Password1!	ModMappings	bob	1579206077013	\N	0
\.


--
-- Data for Name: user_federation_config; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.user_federation_config (user_federation_provider_id, value, name) FROM stdin;
\.


--
-- Data for Name: user_federation_mapper; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.user_federation_mapper (id, name, federation_provider_id, federation_mapper_type, realm_id) FROM stdin;
\.


--
-- Data for Name: user_federation_mapper_config; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.user_federation_mapper_config (user_federation_mapper_id, value, name) FROM stdin;
\.


--
-- Data for Name: user_federation_provider; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.user_federation_provider (id, changed_sync_period, display_name, full_sync_period, last_sync, priority, provider_name, realm_id) FROM stdin;
\.


--
-- Data for Name: user_group_membership; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.user_group_membership (group_id, user_id) FROM stdin;
\.


--
-- Data for Name: user_required_action; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.user_required_action (user_id, required_action) FROM stdin;
\.


--
-- Data for Name: user_role_mapping; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.user_role_mapping (role_id, user_id) FROM stdin;
5726c14b-12e1-4acf-ab64-a55d60b8c582	5cb3ca77-47ab-4721-a1ad-7b1868559aa5
7ffebb0e-73d8-4f0b-acee-44b682519130	5cb3ca77-47ab-4721-a1ad-7b1868559aa5
06b6040e-2d97-4054-92f6-aab17984a8fe	5cb3ca77-47ab-4721-a1ad-7b1868559aa5
f2413a59-2051-4fde-85a2-6c4b62d4d6b6	5cb3ca77-47ab-4721-a1ad-7b1868559aa5
b21192ad-92c5-4b28-a87b-361e66aab625	5cb3ca77-47ab-4721-a1ad-7b1868559aa5
9e9c4679-a175-4a04-b72b-234686a133b6	9df4d504-9378-433e-b4e9-3e5a6600f0f1
625aa352-faf9-4bfe-99f1-5f270200eb75	9df4d504-9378-433e-b4e9-3e5a6600f0f1
770e4d48-d5a7-4795-8818-af03c8cf2d14	9df4d504-9378-433e-b4e9-3e5a6600f0f1
c82225bf-240c-444a-b715-57bb9229718f	9df4d504-9378-433e-b4e9-3e5a6600f0f1
6603554d-5faf-4593-ba06-8375360ff166	9df4d504-9378-433e-b4e9-3e5a6600f0f1
9e9c4679-a175-4a04-b72b-234686a133b6	5c150731-d9e2-41c0-8091-836e67699cf1
625aa352-faf9-4bfe-99f1-5f270200eb75	5c150731-d9e2-41c0-8091-836e67699cf1
770e4d48-d5a7-4795-8818-af03c8cf2d14	5c150731-d9e2-41c0-8091-836e67699cf1
c82225bf-240c-444a-b715-57bb9229718f	5c150731-d9e2-41c0-8091-836e67699cf1
3256738e-6f4c-4a0f-bc37-01655a34978b	5c150731-d9e2-41c0-8091-836e67699cf1
\.


--
-- Data for Name: user_session; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.user_session (id, auth_method, ip_address, last_session_refresh, login_username, realm_id, remember_me, started, user_id, user_session_state, broker_session_id, broker_user_id) FROM stdin;
\.


--
-- Data for Name: user_session_note; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.user_session_note (user_session, name, value) FROM stdin;
\.


--
-- Data for Name: username_login_failure; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.username_login_failure (realm_id, username, failed_login_not_before, last_failure, last_ip_failure, num_failures) FROM stdin;
\.


--
-- Data for Name: web_origins; Type: TABLE DATA; Schema: public; Owner: keycloak
--

COPY public.web_origins (client_id, value) FROM stdin;
24463fff-1058-4fc9-8c1e-9ecbcf7fa2e9	+
d8945854-74b3-460b-9089-00fbc8c14672	+
aed69ab6-ca48-4b7f-a94d-629361b04bb8	https://api.modmappings.org
\.


--
-- Name: username_login_failure CONSTRAINT_17-2; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.username_login_failure
    ADD CONSTRAINT "CONSTRAINT_17-2" PRIMARY KEY (realm_id, username);


--
-- Name: keycloak_role UK_J3RWUVD56ONTGSUHOGM184WW2-2; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.keycloak_role
    ADD CONSTRAINT "UK_J3RWUVD56ONTGSUHOGM184WW2-2" UNIQUE (name, client_realm_constraint);


--
-- Name: client_auth_flow_bindings c_cli_flow_bind; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_auth_flow_bindings
    ADD CONSTRAINT c_cli_flow_bind PRIMARY KEY (client_id, binding_name);


--
-- Name: client_scope_client c_cli_scope_bind; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_scope_client
    ADD CONSTRAINT c_cli_scope_bind PRIMARY KEY (client_id, scope_id);


--
-- Name: client_initial_access cnstr_client_init_acc_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_initial_access
    ADD CONSTRAINT cnstr_client_init_acc_pk PRIMARY KEY (id);


--
-- Name: realm_default_groups con_group_id_def_groups; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_default_groups
    ADD CONSTRAINT con_group_id_def_groups UNIQUE (group_id);


--
-- Name: broker_link constr_broker_link_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.broker_link
    ADD CONSTRAINT constr_broker_link_pk PRIMARY KEY (identity_provider, user_id);


--
-- Name: client_user_session_note constr_cl_usr_ses_note; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_user_session_note
    ADD CONSTRAINT constr_cl_usr_ses_note PRIMARY KEY (client_session, name);


--
-- Name: client_default_roles constr_client_default_roles; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_default_roles
    ADD CONSTRAINT constr_client_default_roles PRIMARY KEY (client_id, role_id);


--
-- Name: component_config constr_component_config_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.component_config
    ADD CONSTRAINT constr_component_config_pk PRIMARY KEY (id);


--
-- Name: component constr_component_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.component
    ADD CONSTRAINT constr_component_pk PRIMARY KEY (id);


--
-- Name: fed_user_required_action constr_fed_required_action; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.fed_user_required_action
    ADD CONSTRAINT constr_fed_required_action PRIMARY KEY (required_action, user_id);


--
-- Name: fed_user_attribute constr_fed_user_attr_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.fed_user_attribute
    ADD CONSTRAINT constr_fed_user_attr_pk PRIMARY KEY (id);


--
-- Name: fed_user_consent constr_fed_user_consent_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.fed_user_consent
    ADD CONSTRAINT constr_fed_user_consent_pk PRIMARY KEY (id);


--
-- Name: fed_user_credential constr_fed_user_cred_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.fed_user_credential
    ADD CONSTRAINT constr_fed_user_cred_pk PRIMARY KEY (id);


--
-- Name: fed_user_group_membership constr_fed_user_group; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.fed_user_group_membership
    ADD CONSTRAINT constr_fed_user_group PRIMARY KEY (group_id, user_id);


--
-- Name: fed_user_role_mapping constr_fed_user_role; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.fed_user_role_mapping
    ADD CONSTRAINT constr_fed_user_role PRIMARY KEY (role_id, user_id);


--
-- Name: federated_user constr_federated_user; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.federated_user
    ADD CONSTRAINT constr_federated_user PRIMARY KEY (id);


--
-- Name: realm_default_groups constr_realm_default_groups; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_default_groups
    ADD CONSTRAINT constr_realm_default_groups PRIMARY KEY (realm_id, group_id);


--
-- Name: realm_enabled_event_types constr_realm_enabl_event_types; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_enabled_event_types
    ADD CONSTRAINT constr_realm_enabl_event_types PRIMARY KEY (realm_id, value);


--
-- Name: realm_events_listeners constr_realm_events_listeners; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_events_listeners
    ADD CONSTRAINT constr_realm_events_listeners PRIMARY KEY (realm_id, value);


--
-- Name: realm_supported_locales constr_realm_supported_locales; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_supported_locales
    ADD CONSTRAINT constr_realm_supported_locales PRIMARY KEY (realm_id, value);


--
-- Name: identity_provider constraint_2b; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.identity_provider
    ADD CONSTRAINT constraint_2b PRIMARY KEY (internal_id);


--
-- Name: client_attributes constraint_3c; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_attributes
    ADD CONSTRAINT constraint_3c PRIMARY KEY (client_id, name);


--
-- Name: event_entity constraint_4; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.event_entity
    ADD CONSTRAINT constraint_4 PRIMARY KEY (id);


--
-- Name: federated_identity constraint_40; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.federated_identity
    ADD CONSTRAINT constraint_40 PRIMARY KEY (identity_provider, user_id);


--
-- Name: realm constraint_4a; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm
    ADD CONSTRAINT constraint_4a PRIMARY KEY (id);


--
-- Name: client_session_role constraint_5; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_session_role
    ADD CONSTRAINT constraint_5 PRIMARY KEY (client_session, role_id);


--
-- Name: user_session constraint_57; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_session
    ADD CONSTRAINT constraint_57 PRIMARY KEY (id);


--
-- Name: user_federation_provider constraint_5c; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_federation_provider
    ADD CONSTRAINT constraint_5c PRIMARY KEY (id);


--
-- Name: client_session_note constraint_5e; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_session_note
    ADD CONSTRAINT constraint_5e PRIMARY KEY (client_session, name);


--
-- Name: client constraint_7; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client
    ADD CONSTRAINT constraint_7 PRIMARY KEY (id);


--
-- Name: client_session constraint_8; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_session
    ADD CONSTRAINT constraint_8 PRIMARY KEY (id);


--
-- Name: scope_mapping constraint_81; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.scope_mapping
    ADD CONSTRAINT constraint_81 PRIMARY KEY (client_id, role_id);


--
-- Name: client_node_registrations constraint_84; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_node_registrations
    ADD CONSTRAINT constraint_84 PRIMARY KEY (client_id, name);


--
-- Name: realm_attribute constraint_9; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_attribute
    ADD CONSTRAINT constraint_9 PRIMARY KEY (name, realm_id);


--
-- Name: realm_required_credential constraint_92; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_required_credential
    ADD CONSTRAINT constraint_92 PRIMARY KEY (realm_id, type);


--
-- Name: keycloak_role constraint_a; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.keycloak_role
    ADD CONSTRAINT constraint_a PRIMARY KEY (id);


--
-- Name: admin_event_entity constraint_admin_event_entity; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.admin_event_entity
    ADD CONSTRAINT constraint_admin_event_entity PRIMARY KEY (id);


--
-- Name: authenticator_config_entry constraint_auth_cfg_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.authenticator_config_entry
    ADD CONSTRAINT constraint_auth_cfg_pk PRIMARY KEY (authenticator_id, name);


--
-- Name: authentication_execution constraint_auth_exec_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.authentication_execution
    ADD CONSTRAINT constraint_auth_exec_pk PRIMARY KEY (id);


--
-- Name: authentication_flow constraint_auth_flow_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.authentication_flow
    ADD CONSTRAINT constraint_auth_flow_pk PRIMARY KEY (id);


--
-- Name: authenticator_config constraint_auth_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.authenticator_config
    ADD CONSTRAINT constraint_auth_pk PRIMARY KEY (id);


--
-- Name: client_session_auth_status constraint_auth_status_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_session_auth_status
    ADD CONSTRAINT constraint_auth_status_pk PRIMARY KEY (client_session, authenticator);


--
-- Name: user_role_mapping constraint_c; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_role_mapping
    ADD CONSTRAINT constraint_c PRIMARY KEY (role_id, user_id);


--
-- Name: composite_role constraint_composite_role; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.composite_role
    ADD CONSTRAINT constraint_composite_role PRIMARY KEY (composite, child_role);


--
-- Name: client_session_prot_mapper constraint_cs_pmp_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_session_prot_mapper
    ADD CONSTRAINT constraint_cs_pmp_pk PRIMARY KEY (client_session, protocol_mapper_id);


--
-- Name: identity_provider_config constraint_d; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.identity_provider_config
    ADD CONSTRAINT constraint_d PRIMARY KEY (identity_provider_id, name);


--
-- Name: policy_config constraint_dpc; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.policy_config
    ADD CONSTRAINT constraint_dpc PRIMARY KEY (policy_id, name);


--
-- Name: realm_smtp_config constraint_e; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_smtp_config
    ADD CONSTRAINT constraint_e PRIMARY KEY (realm_id, name);


--
-- Name: credential constraint_f; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.credential
    ADD CONSTRAINT constraint_f PRIMARY KEY (id);


--
-- Name: user_federation_config constraint_f9; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_federation_config
    ADD CONSTRAINT constraint_f9 PRIMARY KEY (user_federation_provider_id, name);


--
-- Name: resource_server_perm_ticket constraint_fapmt; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_server_perm_ticket
    ADD CONSTRAINT constraint_fapmt PRIMARY KEY (id);


--
-- Name: resource_server_resource constraint_farsr; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_server_resource
    ADD CONSTRAINT constraint_farsr PRIMARY KEY (id);


--
-- Name: resource_server_policy constraint_farsrp; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_server_policy
    ADD CONSTRAINT constraint_farsrp PRIMARY KEY (id);


--
-- Name: associated_policy constraint_farsrpap; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.associated_policy
    ADD CONSTRAINT constraint_farsrpap PRIMARY KEY (policy_id, associated_policy_id);


--
-- Name: resource_policy constraint_farsrpp; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_policy
    ADD CONSTRAINT constraint_farsrpp PRIMARY KEY (resource_id, policy_id);


--
-- Name: resource_server_scope constraint_farsrs; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_server_scope
    ADD CONSTRAINT constraint_farsrs PRIMARY KEY (id);


--
-- Name: resource_scope constraint_farsrsp; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_scope
    ADD CONSTRAINT constraint_farsrsp PRIMARY KEY (resource_id, scope_id);


--
-- Name: scope_policy constraint_farsrsps; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.scope_policy
    ADD CONSTRAINT constraint_farsrsps PRIMARY KEY (scope_id, policy_id);


--
-- Name: user_entity constraint_fb; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_entity
    ADD CONSTRAINT constraint_fb PRIMARY KEY (id);


--
-- Name: user_federation_mapper_config constraint_fedmapper_cfg_pm; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_federation_mapper_config
    ADD CONSTRAINT constraint_fedmapper_cfg_pm PRIMARY KEY (user_federation_mapper_id, name);


--
-- Name: user_federation_mapper constraint_fedmapperpm; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_federation_mapper
    ADD CONSTRAINT constraint_fedmapperpm PRIMARY KEY (id);


--
-- Name: fed_user_consent_cl_scope constraint_fgrntcsnt_clsc_pm; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.fed_user_consent_cl_scope
    ADD CONSTRAINT constraint_fgrntcsnt_clsc_pm PRIMARY KEY (user_consent_id, scope_id);


--
-- Name: user_consent_client_scope constraint_grntcsnt_clsc_pm; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_consent_client_scope
    ADD CONSTRAINT constraint_grntcsnt_clsc_pm PRIMARY KEY (user_consent_id, scope_id);


--
-- Name: user_consent constraint_grntcsnt_pm; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_consent
    ADD CONSTRAINT constraint_grntcsnt_pm PRIMARY KEY (id);


--
-- Name: keycloak_group constraint_group; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.keycloak_group
    ADD CONSTRAINT constraint_group PRIMARY KEY (id);


--
-- Name: group_attribute constraint_group_attribute_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.group_attribute
    ADD CONSTRAINT constraint_group_attribute_pk PRIMARY KEY (id);


--
-- Name: group_role_mapping constraint_group_role; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.group_role_mapping
    ADD CONSTRAINT constraint_group_role PRIMARY KEY (role_id, group_id);


--
-- Name: identity_provider_mapper constraint_idpm; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.identity_provider_mapper
    ADD CONSTRAINT constraint_idpm PRIMARY KEY (id);


--
-- Name: idp_mapper_config constraint_idpmconfig; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.idp_mapper_config
    ADD CONSTRAINT constraint_idpmconfig PRIMARY KEY (idp_mapper_id, name);


--
-- Name: migration_model constraint_migmod; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.migration_model
    ADD CONSTRAINT constraint_migmod PRIMARY KEY (id);


--
-- Name: offline_client_session constraint_offl_cl_ses_pk3; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.offline_client_session
    ADD CONSTRAINT constraint_offl_cl_ses_pk3 PRIMARY KEY (user_session_id, client_id, client_storage_provider, external_client_id, offline_flag);


--
-- Name: offline_user_session constraint_offl_us_ses_pk2; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.offline_user_session
    ADD CONSTRAINT constraint_offl_us_ses_pk2 PRIMARY KEY (user_session_id, offline_flag);


--
-- Name: protocol_mapper constraint_pcm; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.protocol_mapper
    ADD CONSTRAINT constraint_pcm PRIMARY KEY (id);


--
-- Name: protocol_mapper_config constraint_pmconfig; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.protocol_mapper_config
    ADD CONSTRAINT constraint_pmconfig PRIMARY KEY (protocol_mapper_id, name);


--
-- Name: realm_default_roles constraint_realm_default_roles; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_default_roles
    ADD CONSTRAINT constraint_realm_default_roles PRIMARY KEY (realm_id, role_id);


--
-- Name: redirect_uris constraint_redirect_uris; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.redirect_uris
    ADD CONSTRAINT constraint_redirect_uris PRIMARY KEY (client_id, value);


--
-- Name: required_action_config constraint_req_act_cfg_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.required_action_config
    ADD CONSTRAINT constraint_req_act_cfg_pk PRIMARY KEY (required_action_id, name);


--
-- Name: required_action_provider constraint_req_act_prv_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.required_action_provider
    ADD CONSTRAINT constraint_req_act_prv_pk PRIMARY KEY (id);


--
-- Name: user_required_action constraint_required_action; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_required_action
    ADD CONSTRAINT constraint_required_action PRIMARY KEY (required_action, user_id);


--
-- Name: resource_uris constraint_resour_uris_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_uris
    ADD CONSTRAINT constraint_resour_uris_pk PRIMARY KEY (resource_id, value);


--
-- Name: role_attribute constraint_role_attribute_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.role_attribute
    ADD CONSTRAINT constraint_role_attribute_pk PRIMARY KEY (id);


--
-- Name: user_attribute constraint_user_attribute_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_attribute
    ADD CONSTRAINT constraint_user_attribute_pk PRIMARY KEY (id);


--
-- Name: user_group_membership constraint_user_group; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_group_membership
    ADD CONSTRAINT constraint_user_group PRIMARY KEY (group_id, user_id);


--
-- Name: user_session_note constraint_usn_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_session_note
    ADD CONSTRAINT constraint_usn_pk PRIMARY KEY (user_session, name);


--
-- Name: web_origins constraint_web_origins; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.web_origins
    ADD CONSTRAINT constraint_web_origins PRIMARY KEY (client_id, value);


--
-- Name: client_scope_attributes pk_cl_tmpl_attr; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_scope_attributes
    ADD CONSTRAINT pk_cl_tmpl_attr PRIMARY KEY (scope_id, name);


--
-- Name: client_scope pk_cli_template; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_scope
    ADD CONSTRAINT pk_cli_template PRIMARY KEY (id);


--
-- Name: databasechangeloglock pk_databasechangeloglock; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.databasechangeloglock
    ADD CONSTRAINT pk_databasechangeloglock PRIMARY KEY (id);


--
-- Name: resource_server pk_resource_server; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_server
    ADD CONSTRAINT pk_resource_server PRIMARY KEY (id);


--
-- Name: client_scope_role_mapping pk_template_scope; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_scope_role_mapping
    ADD CONSTRAINT pk_template_scope PRIMARY KEY (scope_id, role_id);


--
-- Name: default_client_scope r_def_cli_scope_bind; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.default_client_scope
    ADD CONSTRAINT r_def_cli_scope_bind PRIMARY KEY (realm_id, scope_id);


--
-- Name: resource_attribute res_attr_pk; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_attribute
    ADD CONSTRAINT res_attr_pk PRIMARY KEY (id);


--
-- Name: keycloak_group sibling_names; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.keycloak_group
    ADD CONSTRAINT sibling_names UNIQUE (realm_id, parent_group, name);


--
-- Name: identity_provider uk_2daelwnibji49avxsrtuf6xj33; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.identity_provider
    ADD CONSTRAINT uk_2daelwnibji49avxsrtuf6xj33 UNIQUE (provider_alias, realm_id);


--
-- Name: client_default_roles uk_8aelwnibji49avxsrtuf6xjow; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_default_roles
    ADD CONSTRAINT uk_8aelwnibji49avxsrtuf6xjow UNIQUE (role_id);


--
-- Name: client uk_b71cjlbenv945rb6gcon438at; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client
    ADD CONSTRAINT uk_b71cjlbenv945rb6gcon438at UNIQUE (realm_id, client_id);


--
-- Name: client_scope uk_cli_scope; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_scope
    ADD CONSTRAINT uk_cli_scope UNIQUE (realm_id, name);


--
-- Name: user_entity uk_dykn684sl8up1crfei6eckhd7; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_entity
    ADD CONSTRAINT uk_dykn684sl8up1crfei6eckhd7 UNIQUE (realm_id, email_constraint);


--
-- Name: resource_server_resource uk_frsr6t700s9v50bu18ws5ha6; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_server_resource
    ADD CONSTRAINT uk_frsr6t700s9v50bu18ws5ha6 UNIQUE (name, owner, resource_server_id);


--
-- Name: resource_server_perm_ticket uk_frsr6t700s9v50bu18ws5pmt; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_server_perm_ticket
    ADD CONSTRAINT uk_frsr6t700s9v50bu18ws5pmt UNIQUE (owner, requester, resource_server_id, resource_id, scope_id);


--
-- Name: resource_server_policy uk_frsrpt700s9v50bu18ws5ha6; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_server_policy
    ADD CONSTRAINT uk_frsrpt700s9v50bu18ws5ha6 UNIQUE (name, resource_server_id);


--
-- Name: resource_server_scope uk_frsrst700s9v50bu18ws5ha6; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_server_scope
    ADD CONSTRAINT uk_frsrst700s9v50bu18ws5ha6 UNIQUE (name, resource_server_id);


--
-- Name: realm_default_roles uk_h4wpd7w4hsoolni3h0sw7btje; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_default_roles
    ADD CONSTRAINT uk_h4wpd7w4hsoolni3h0sw7btje UNIQUE (role_id);


--
-- Name: user_consent uk_jkuwuvd56ontgsuhogm8uewrt; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_consent
    ADD CONSTRAINT uk_jkuwuvd56ontgsuhogm8uewrt UNIQUE (client_id, client_storage_provider, external_client_id, user_id);


--
-- Name: realm uk_orvsdmla56612eaefiq6wl5oi; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm
    ADD CONSTRAINT uk_orvsdmla56612eaefiq6wl5oi UNIQUE (name);


--
-- Name: user_entity uk_ru8tt6t700s9v50bu18ws5ha6; Type: CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_entity
    ADD CONSTRAINT uk_ru8tt6t700s9v50bu18ws5ha6 UNIQUE (realm_id, username);


--
-- Name: idx_assoc_pol_assoc_pol_id; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_assoc_pol_assoc_pol_id ON public.associated_policy USING btree (associated_policy_id);


--
-- Name: idx_auth_config_realm; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_auth_config_realm ON public.authenticator_config USING btree (realm_id);


--
-- Name: idx_auth_exec_flow; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_auth_exec_flow ON public.authentication_execution USING btree (flow_id);


--
-- Name: idx_auth_exec_realm_flow; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_auth_exec_realm_flow ON public.authentication_execution USING btree (realm_id, flow_id);


--
-- Name: idx_auth_flow_realm; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_auth_flow_realm ON public.authentication_flow USING btree (realm_id);


--
-- Name: idx_cl_clscope; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_cl_clscope ON public.client_scope_client USING btree (scope_id);


--
-- Name: idx_client_def_roles_client; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_client_def_roles_client ON public.client_default_roles USING btree (client_id);


--
-- Name: idx_client_init_acc_realm; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_client_init_acc_realm ON public.client_initial_access USING btree (realm_id);


--
-- Name: idx_client_session_session; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_client_session_session ON public.client_session USING btree (session_id);


--
-- Name: idx_clscope_attrs; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_clscope_attrs ON public.client_scope_attributes USING btree (scope_id);


--
-- Name: idx_clscope_cl; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_clscope_cl ON public.client_scope_client USING btree (client_id);


--
-- Name: idx_clscope_protmap; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_clscope_protmap ON public.protocol_mapper USING btree (client_scope_id);


--
-- Name: idx_clscope_role; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_clscope_role ON public.client_scope_role_mapping USING btree (scope_id);


--
-- Name: idx_compo_config_compo; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_compo_config_compo ON public.component_config USING btree (component_id);


--
-- Name: idx_component_provider_type; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_component_provider_type ON public.component USING btree (provider_type);


--
-- Name: idx_component_realm; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_component_realm ON public.component USING btree (realm_id);


--
-- Name: idx_composite; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_composite ON public.composite_role USING btree (composite);


--
-- Name: idx_composite_child; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_composite_child ON public.composite_role USING btree (child_role);


--
-- Name: idx_defcls_realm; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_defcls_realm ON public.default_client_scope USING btree (realm_id);


--
-- Name: idx_defcls_scope; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_defcls_scope ON public.default_client_scope USING btree (scope_id);


--
-- Name: idx_fedidentity_feduser; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_fedidentity_feduser ON public.federated_identity USING btree (federated_user_id);


--
-- Name: idx_fedidentity_user; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_fedidentity_user ON public.federated_identity USING btree (user_id);


--
-- Name: idx_fu_attribute; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_fu_attribute ON public.fed_user_attribute USING btree (user_id, realm_id, name);


--
-- Name: idx_fu_cnsnt_ext; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_fu_cnsnt_ext ON public.fed_user_consent USING btree (user_id, client_storage_provider, external_client_id);


--
-- Name: idx_fu_consent; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_fu_consent ON public.fed_user_consent USING btree (user_id, client_id);


--
-- Name: idx_fu_consent_ru; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_fu_consent_ru ON public.fed_user_consent USING btree (realm_id, user_id);


--
-- Name: idx_fu_credential; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_fu_credential ON public.fed_user_credential USING btree (user_id, type);


--
-- Name: idx_fu_credential_ru; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_fu_credential_ru ON public.fed_user_credential USING btree (realm_id, user_id);


--
-- Name: idx_fu_group_membership; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_fu_group_membership ON public.fed_user_group_membership USING btree (user_id, group_id);


--
-- Name: idx_fu_group_membership_ru; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_fu_group_membership_ru ON public.fed_user_group_membership USING btree (realm_id, user_id);


--
-- Name: idx_fu_required_action; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_fu_required_action ON public.fed_user_required_action USING btree (user_id, required_action);


--
-- Name: idx_fu_required_action_ru; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_fu_required_action_ru ON public.fed_user_required_action USING btree (realm_id, user_id);


--
-- Name: idx_fu_role_mapping; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_fu_role_mapping ON public.fed_user_role_mapping USING btree (user_id, role_id);


--
-- Name: idx_fu_role_mapping_ru; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_fu_role_mapping_ru ON public.fed_user_role_mapping USING btree (realm_id, user_id);


--
-- Name: idx_group_attr_group; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_group_attr_group ON public.group_attribute USING btree (group_id);


--
-- Name: idx_group_role_mapp_group; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_group_role_mapp_group ON public.group_role_mapping USING btree (group_id);


--
-- Name: idx_id_prov_mapp_realm; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_id_prov_mapp_realm ON public.identity_provider_mapper USING btree (realm_id);


--
-- Name: idx_ident_prov_realm; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_ident_prov_realm ON public.identity_provider USING btree (realm_id);


--
-- Name: idx_keycloak_role_client; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_keycloak_role_client ON public.keycloak_role USING btree (client);


--
-- Name: idx_keycloak_role_realm; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_keycloak_role_realm ON public.keycloak_role USING btree (realm);


--
-- Name: idx_offline_uss_createdon; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_offline_uss_createdon ON public.offline_user_session USING btree (created_on);


--
-- Name: idx_protocol_mapper_client; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_protocol_mapper_client ON public.protocol_mapper USING btree (client_id);


--
-- Name: idx_realm_attr_realm; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_realm_attr_realm ON public.realm_attribute USING btree (realm_id);


--
-- Name: idx_realm_clscope; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_realm_clscope ON public.client_scope USING btree (realm_id);


--
-- Name: idx_realm_def_grp_realm; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_realm_def_grp_realm ON public.realm_default_groups USING btree (realm_id);


--
-- Name: idx_realm_def_roles_realm; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_realm_def_roles_realm ON public.realm_default_roles USING btree (realm_id);


--
-- Name: idx_realm_evt_list_realm; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_realm_evt_list_realm ON public.realm_events_listeners USING btree (realm_id);


--
-- Name: idx_realm_evt_types_realm; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_realm_evt_types_realm ON public.realm_enabled_event_types USING btree (realm_id);


--
-- Name: idx_realm_master_adm_cli; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_realm_master_adm_cli ON public.realm USING btree (master_admin_client);


--
-- Name: idx_realm_supp_local_realm; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_realm_supp_local_realm ON public.realm_supported_locales USING btree (realm_id);


--
-- Name: idx_redir_uri_client; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_redir_uri_client ON public.redirect_uris USING btree (client_id);


--
-- Name: idx_req_act_prov_realm; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_req_act_prov_realm ON public.required_action_provider USING btree (realm_id);


--
-- Name: idx_res_policy_policy; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_res_policy_policy ON public.resource_policy USING btree (policy_id);


--
-- Name: idx_res_scope_scope; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_res_scope_scope ON public.resource_scope USING btree (scope_id);


--
-- Name: idx_res_serv_pol_res_serv; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_res_serv_pol_res_serv ON public.resource_server_policy USING btree (resource_server_id);


--
-- Name: idx_res_srv_res_res_srv; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_res_srv_res_res_srv ON public.resource_server_resource USING btree (resource_server_id);


--
-- Name: idx_res_srv_scope_res_srv; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_res_srv_scope_res_srv ON public.resource_server_scope USING btree (resource_server_id);


--
-- Name: idx_role_attribute; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_role_attribute ON public.role_attribute USING btree (role_id);


--
-- Name: idx_role_clscope; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_role_clscope ON public.client_scope_role_mapping USING btree (role_id);


--
-- Name: idx_scope_mapping_role; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_scope_mapping_role ON public.scope_mapping USING btree (role_id);


--
-- Name: idx_scope_policy_policy; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_scope_policy_policy ON public.scope_policy USING btree (policy_id);


--
-- Name: idx_update_time; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_update_time ON public.migration_model USING btree (update_time);


--
-- Name: idx_us_sess_id_on_cl_sess; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_us_sess_id_on_cl_sess ON public.offline_client_session USING btree (user_session_id);


--
-- Name: idx_usconsent_clscope; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_usconsent_clscope ON public.user_consent_client_scope USING btree (user_consent_id);


--
-- Name: idx_user_attribute; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_user_attribute ON public.user_attribute USING btree (user_id);


--
-- Name: idx_user_consent; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_user_consent ON public.user_consent USING btree (user_id);


--
-- Name: idx_user_credential; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_user_credential ON public.credential USING btree (user_id);


--
-- Name: idx_user_email; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_user_email ON public.user_entity USING btree (email);


--
-- Name: idx_user_group_mapping; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_user_group_mapping ON public.user_group_membership USING btree (user_id);


--
-- Name: idx_user_reqactions; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_user_reqactions ON public.user_required_action USING btree (user_id);


--
-- Name: idx_user_role_mapping; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_user_role_mapping ON public.user_role_mapping USING btree (user_id);


--
-- Name: idx_usr_fed_map_fed_prv; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_usr_fed_map_fed_prv ON public.user_federation_mapper USING btree (federation_provider_id);


--
-- Name: idx_usr_fed_map_realm; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_usr_fed_map_realm ON public.user_federation_mapper USING btree (realm_id);


--
-- Name: idx_usr_fed_prv_realm; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_usr_fed_prv_realm ON public.user_federation_provider USING btree (realm_id);


--
-- Name: idx_web_orig_client; Type: INDEX; Schema: public; Owner: keycloak
--

CREATE INDEX idx_web_orig_client ON public.web_origins USING btree (client_id);


--
-- Name: client_session_auth_status auth_status_constraint; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_session_auth_status
    ADD CONSTRAINT auth_status_constraint FOREIGN KEY (client_session) REFERENCES public.client_session(id);


--
-- Name: identity_provider fk2b4ebc52ae5c3b34; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.identity_provider
    ADD CONSTRAINT fk2b4ebc52ae5c3b34 FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: client_attributes fk3c47c64beacca966; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_attributes
    ADD CONSTRAINT fk3c47c64beacca966 FOREIGN KEY (client_id) REFERENCES public.client(id);


--
-- Name: federated_identity fk404288b92ef007a6; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.federated_identity
    ADD CONSTRAINT fk404288b92ef007a6 FOREIGN KEY (user_id) REFERENCES public.user_entity(id);


--
-- Name: client_node_registrations fk4129723ba992f594; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_node_registrations
    ADD CONSTRAINT fk4129723ba992f594 FOREIGN KEY (client_id) REFERENCES public.client(id);


--
-- Name: client_session_note fk5edfb00ff51c2736; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_session_note
    ADD CONSTRAINT fk5edfb00ff51c2736 FOREIGN KEY (client_session) REFERENCES public.client_session(id);


--
-- Name: user_session_note fk5edfb00ff51d3472; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_session_note
    ADD CONSTRAINT fk5edfb00ff51d3472 FOREIGN KEY (user_session) REFERENCES public.user_session(id);


--
-- Name: client_session_role fk_11b7sgqw18i532811v7o2dv76; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_session_role
    ADD CONSTRAINT fk_11b7sgqw18i532811v7o2dv76 FOREIGN KEY (client_session) REFERENCES public.client_session(id);


--
-- Name: redirect_uris fk_1burs8pb4ouj97h5wuppahv9f; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.redirect_uris
    ADD CONSTRAINT fk_1burs8pb4ouj97h5wuppahv9f FOREIGN KEY (client_id) REFERENCES public.client(id);


--
-- Name: user_federation_provider fk_1fj32f6ptolw2qy60cd8n01e8; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_federation_provider
    ADD CONSTRAINT fk_1fj32f6ptolw2qy60cd8n01e8 FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: client_session_prot_mapper fk_33a8sgqw18i532811v7o2dk89; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_session_prot_mapper
    ADD CONSTRAINT fk_33a8sgqw18i532811v7o2dk89 FOREIGN KEY (client_session) REFERENCES public.client_session(id);


--
-- Name: realm_required_credential fk_5hg65lybevavkqfki3kponh9v; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_required_credential
    ADD CONSTRAINT fk_5hg65lybevavkqfki3kponh9v FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: resource_attribute fk_5hrm2vlf9ql5fu022kqepovbr; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_attribute
    ADD CONSTRAINT fk_5hrm2vlf9ql5fu022kqepovbr FOREIGN KEY (resource_id) REFERENCES public.resource_server_resource(id);


--
-- Name: user_attribute fk_5hrm2vlf9ql5fu043kqepovbr; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_attribute
    ADD CONSTRAINT fk_5hrm2vlf9ql5fu043kqepovbr FOREIGN KEY (user_id) REFERENCES public.user_entity(id);


--
-- Name: user_required_action fk_6qj3w1jw9cvafhe19bwsiuvmd; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_required_action
    ADD CONSTRAINT fk_6qj3w1jw9cvafhe19bwsiuvmd FOREIGN KEY (user_id) REFERENCES public.user_entity(id);


--
-- Name: keycloak_role fk_6vyqfe4cn4wlq8r6kt5vdsj5c; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.keycloak_role
    ADD CONSTRAINT fk_6vyqfe4cn4wlq8r6kt5vdsj5c FOREIGN KEY (realm) REFERENCES public.realm(id);


--
-- Name: realm_smtp_config fk_70ej8xdxgxd0b9hh6180irr0o; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_smtp_config
    ADD CONSTRAINT fk_70ej8xdxgxd0b9hh6180irr0o FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: client_default_roles fk_8aelwnibji49avxsrtuf6xjow; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_default_roles
    ADD CONSTRAINT fk_8aelwnibji49avxsrtuf6xjow FOREIGN KEY (role_id) REFERENCES public.keycloak_role(id);


--
-- Name: realm_attribute fk_8shxd6l3e9atqukacxgpffptw; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_attribute
    ADD CONSTRAINT fk_8shxd6l3e9atqukacxgpffptw FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: composite_role fk_a63wvekftu8jo1pnj81e7mce2; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.composite_role
    ADD CONSTRAINT fk_a63wvekftu8jo1pnj81e7mce2 FOREIGN KEY (composite) REFERENCES public.keycloak_role(id);


--
-- Name: authentication_execution fk_auth_exec_flow; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.authentication_execution
    ADD CONSTRAINT fk_auth_exec_flow FOREIGN KEY (flow_id) REFERENCES public.authentication_flow(id);


--
-- Name: authentication_execution fk_auth_exec_realm; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.authentication_execution
    ADD CONSTRAINT fk_auth_exec_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: authentication_flow fk_auth_flow_realm; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.authentication_flow
    ADD CONSTRAINT fk_auth_flow_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: authenticator_config fk_auth_realm; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.authenticator_config
    ADD CONSTRAINT fk_auth_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: client_session fk_b4ao2vcvat6ukau74wbwtfqo1; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_session
    ADD CONSTRAINT fk_b4ao2vcvat6ukau74wbwtfqo1 FOREIGN KEY (session_id) REFERENCES public.user_session(id);


--
-- Name: user_role_mapping fk_c4fqv34p1mbylloxang7b1q3l; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_role_mapping
    ADD CONSTRAINT fk_c4fqv34p1mbylloxang7b1q3l FOREIGN KEY (user_id) REFERENCES public.user_entity(id);


--
-- Name: client_scope_client fk_c_cli_scope_client; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_scope_client
    ADD CONSTRAINT fk_c_cli_scope_client FOREIGN KEY (client_id) REFERENCES public.client(id);


--
-- Name: client_scope_client fk_c_cli_scope_scope; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_scope_client
    ADD CONSTRAINT fk_c_cli_scope_scope FOREIGN KEY (scope_id) REFERENCES public.client_scope(id);


--
-- Name: client_scope_attributes fk_cl_scope_attr_scope; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_scope_attributes
    ADD CONSTRAINT fk_cl_scope_attr_scope FOREIGN KEY (scope_id) REFERENCES public.client_scope(id);


--
-- Name: client_scope_role_mapping fk_cl_scope_rm_role; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_scope_role_mapping
    ADD CONSTRAINT fk_cl_scope_rm_role FOREIGN KEY (role_id) REFERENCES public.keycloak_role(id);


--
-- Name: client_scope_role_mapping fk_cl_scope_rm_scope; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_scope_role_mapping
    ADD CONSTRAINT fk_cl_scope_rm_scope FOREIGN KEY (scope_id) REFERENCES public.client_scope(id);


--
-- Name: client_user_session_note fk_cl_usr_ses_note; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_user_session_note
    ADD CONSTRAINT fk_cl_usr_ses_note FOREIGN KEY (client_session) REFERENCES public.client_session(id);


--
-- Name: protocol_mapper fk_cli_scope_mapper; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.protocol_mapper
    ADD CONSTRAINT fk_cli_scope_mapper FOREIGN KEY (client_scope_id) REFERENCES public.client_scope(id);


--
-- Name: client_initial_access fk_client_init_acc_realm; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_initial_access
    ADD CONSTRAINT fk_client_init_acc_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: component_config fk_component_config; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.component_config
    ADD CONSTRAINT fk_component_config FOREIGN KEY (component_id) REFERENCES public.component(id);


--
-- Name: component fk_component_realm; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.component
    ADD CONSTRAINT fk_component_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: realm_default_groups fk_def_groups_group; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_default_groups
    ADD CONSTRAINT fk_def_groups_group FOREIGN KEY (group_id) REFERENCES public.keycloak_group(id);


--
-- Name: realm_default_groups fk_def_groups_realm; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_default_groups
    ADD CONSTRAINT fk_def_groups_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: realm_default_roles fk_evudb1ppw84oxfax2drs03icc; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_default_roles
    ADD CONSTRAINT fk_evudb1ppw84oxfax2drs03icc FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: user_federation_mapper_config fk_fedmapper_cfg; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_federation_mapper_config
    ADD CONSTRAINT fk_fedmapper_cfg FOREIGN KEY (user_federation_mapper_id) REFERENCES public.user_federation_mapper(id);


--
-- Name: user_federation_mapper fk_fedmapperpm_fedprv; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_federation_mapper
    ADD CONSTRAINT fk_fedmapperpm_fedprv FOREIGN KEY (federation_provider_id) REFERENCES public.user_federation_provider(id);


--
-- Name: user_federation_mapper fk_fedmapperpm_realm; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_federation_mapper
    ADD CONSTRAINT fk_fedmapperpm_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: associated_policy fk_frsr5s213xcx4wnkog82ssrfy; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.associated_policy
    ADD CONSTRAINT fk_frsr5s213xcx4wnkog82ssrfy FOREIGN KEY (associated_policy_id) REFERENCES public.resource_server_policy(id);


--
-- Name: scope_policy fk_frsrasp13xcx4wnkog82ssrfy; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.scope_policy
    ADD CONSTRAINT fk_frsrasp13xcx4wnkog82ssrfy FOREIGN KEY (policy_id) REFERENCES public.resource_server_policy(id);


--
-- Name: resource_server_perm_ticket fk_frsrho213xcx4wnkog82sspmt; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_server_perm_ticket
    ADD CONSTRAINT fk_frsrho213xcx4wnkog82sspmt FOREIGN KEY (resource_server_id) REFERENCES public.resource_server(id);


--
-- Name: resource_server_resource fk_frsrho213xcx4wnkog82ssrfy; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_server_resource
    ADD CONSTRAINT fk_frsrho213xcx4wnkog82ssrfy FOREIGN KEY (resource_server_id) REFERENCES public.resource_server(id);


--
-- Name: resource_server_perm_ticket fk_frsrho213xcx4wnkog83sspmt; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_server_perm_ticket
    ADD CONSTRAINT fk_frsrho213xcx4wnkog83sspmt FOREIGN KEY (resource_id) REFERENCES public.resource_server_resource(id);


--
-- Name: resource_server_perm_ticket fk_frsrho213xcx4wnkog84sspmt; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_server_perm_ticket
    ADD CONSTRAINT fk_frsrho213xcx4wnkog84sspmt FOREIGN KEY (scope_id) REFERENCES public.resource_server_scope(id);


--
-- Name: associated_policy fk_frsrpas14xcx4wnkog82ssrfy; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.associated_policy
    ADD CONSTRAINT fk_frsrpas14xcx4wnkog82ssrfy FOREIGN KEY (policy_id) REFERENCES public.resource_server_policy(id);


--
-- Name: scope_policy fk_frsrpass3xcx4wnkog82ssrfy; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.scope_policy
    ADD CONSTRAINT fk_frsrpass3xcx4wnkog82ssrfy FOREIGN KEY (scope_id) REFERENCES public.resource_server_scope(id);


--
-- Name: resource_server_perm_ticket fk_frsrpo2128cx4wnkog82ssrfy; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_server_perm_ticket
    ADD CONSTRAINT fk_frsrpo2128cx4wnkog82ssrfy FOREIGN KEY (policy_id) REFERENCES public.resource_server_policy(id);


--
-- Name: resource_server_policy fk_frsrpo213xcx4wnkog82ssrfy; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_server_policy
    ADD CONSTRAINT fk_frsrpo213xcx4wnkog82ssrfy FOREIGN KEY (resource_server_id) REFERENCES public.resource_server(id);


--
-- Name: resource_scope fk_frsrpos13xcx4wnkog82ssrfy; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_scope
    ADD CONSTRAINT fk_frsrpos13xcx4wnkog82ssrfy FOREIGN KEY (resource_id) REFERENCES public.resource_server_resource(id);


--
-- Name: resource_policy fk_frsrpos53xcx4wnkog82ssrfy; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_policy
    ADD CONSTRAINT fk_frsrpos53xcx4wnkog82ssrfy FOREIGN KEY (resource_id) REFERENCES public.resource_server_resource(id);


--
-- Name: resource_policy fk_frsrpp213xcx4wnkog82ssrfy; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_policy
    ADD CONSTRAINT fk_frsrpp213xcx4wnkog82ssrfy FOREIGN KEY (policy_id) REFERENCES public.resource_server_policy(id);


--
-- Name: resource_scope fk_frsrps213xcx4wnkog82ssrfy; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_scope
    ADD CONSTRAINT fk_frsrps213xcx4wnkog82ssrfy FOREIGN KEY (scope_id) REFERENCES public.resource_server_scope(id);


--
-- Name: resource_server_scope fk_frsrso213xcx4wnkog82ssrfy; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_server_scope
    ADD CONSTRAINT fk_frsrso213xcx4wnkog82ssrfy FOREIGN KEY (resource_server_id) REFERENCES public.resource_server(id);


--
-- Name: composite_role fk_gr7thllb9lu8q4vqa4524jjy8; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.composite_role
    ADD CONSTRAINT fk_gr7thllb9lu8q4vqa4524jjy8 FOREIGN KEY (child_role) REFERENCES public.keycloak_role(id);


--
-- Name: user_consent_client_scope fk_grntcsnt_clsc_usc; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_consent_client_scope
    ADD CONSTRAINT fk_grntcsnt_clsc_usc FOREIGN KEY (user_consent_id) REFERENCES public.user_consent(id);


--
-- Name: user_consent fk_grntcsnt_user; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_consent
    ADD CONSTRAINT fk_grntcsnt_user FOREIGN KEY (user_id) REFERENCES public.user_entity(id);


--
-- Name: group_attribute fk_group_attribute_group; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.group_attribute
    ADD CONSTRAINT fk_group_attribute_group FOREIGN KEY (group_id) REFERENCES public.keycloak_group(id);


--
-- Name: keycloak_group fk_group_realm; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.keycloak_group
    ADD CONSTRAINT fk_group_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: group_role_mapping fk_group_role_group; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.group_role_mapping
    ADD CONSTRAINT fk_group_role_group FOREIGN KEY (group_id) REFERENCES public.keycloak_group(id);


--
-- Name: group_role_mapping fk_group_role_role; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.group_role_mapping
    ADD CONSTRAINT fk_group_role_role FOREIGN KEY (role_id) REFERENCES public.keycloak_role(id);


--
-- Name: realm_default_roles fk_h4wpd7w4hsoolni3h0sw7btje; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_default_roles
    ADD CONSTRAINT fk_h4wpd7w4hsoolni3h0sw7btje FOREIGN KEY (role_id) REFERENCES public.keycloak_role(id);


--
-- Name: realm_enabled_event_types fk_h846o4h0w8epx5nwedrf5y69j; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_enabled_event_types
    ADD CONSTRAINT fk_h846o4h0w8epx5nwedrf5y69j FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: realm_events_listeners fk_h846o4h0w8epx5nxev9f5y69j; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_events_listeners
    ADD CONSTRAINT fk_h846o4h0w8epx5nxev9f5y69j FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: identity_provider_mapper fk_idpm_realm; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.identity_provider_mapper
    ADD CONSTRAINT fk_idpm_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: idp_mapper_config fk_idpmconfig; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.idp_mapper_config
    ADD CONSTRAINT fk_idpmconfig FOREIGN KEY (idp_mapper_id) REFERENCES public.identity_provider_mapper(id);


--
-- Name: keycloak_role fk_kjho5le2c0ral09fl8cm9wfw9; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.keycloak_role
    ADD CONSTRAINT fk_kjho5le2c0ral09fl8cm9wfw9 FOREIGN KEY (client) REFERENCES public.client(id);


--
-- Name: web_origins fk_lojpho213xcx4wnkog82ssrfy; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.web_origins
    ADD CONSTRAINT fk_lojpho213xcx4wnkog82ssrfy FOREIGN KEY (client_id) REFERENCES public.client(id);


--
-- Name: client_default_roles fk_nuilts7klwqw2h8m2b5joytky; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_default_roles
    ADD CONSTRAINT fk_nuilts7klwqw2h8m2b5joytky FOREIGN KEY (client_id) REFERENCES public.client(id);


--
-- Name: scope_mapping fk_ouse064plmlr732lxjcn1q5f1; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.scope_mapping
    ADD CONSTRAINT fk_ouse064plmlr732lxjcn1q5f1 FOREIGN KEY (client_id) REFERENCES public.client(id);


--
-- Name: scope_mapping fk_p3rh9grku11kqfrs4fltt7rnq; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.scope_mapping
    ADD CONSTRAINT fk_p3rh9grku11kqfrs4fltt7rnq FOREIGN KEY (role_id) REFERENCES public.keycloak_role(id);


--
-- Name: client fk_p56ctinxxb9gsk57fo49f9tac; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client
    ADD CONSTRAINT fk_p56ctinxxb9gsk57fo49f9tac FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: protocol_mapper fk_pcm_realm; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.protocol_mapper
    ADD CONSTRAINT fk_pcm_realm FOREIGN KEY (client_id) REFERENCES public.client(id);


--
-- Name: credential fk_pfyr0glasqyl0dei3kl69r6v0; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.credential
    ADD CONSTRAINT fk_pfyr0glasqyl0dei3kl69r6v0 FOREIGN KEY (user_id) REFERENCES public.user_entity(id);


--
-- Name: protocol_mapper_config fk_pmconfig; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.protocol_mapper_config
    ADD CONSTRAINT fk_pmconfig FOREIGN KEY (protocol_mapper_id) REFERENCES public.protocol_mapper(id);


--
-- Name: default_client_scope fk_r_def_cli_scope_realm; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.default_client_scope
    ADD CONSTRAINT fk_r_def_cli_scope_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: default_client_scope fk_r_def_cli_scope_scope; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.default_client_scope
    ADD CONSTRAINT fk_r_def_cli_scope_scope FOREIGN KEY (scope_id) REFERENCES public.client_scope(id);


--
-- Name: client_scope fk_realm_cli_scope; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.client_scope
    ADD CONSTRAINT fk_realm_cli_scope FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: required_action_provider fk_req_act_realm; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.required_action_provider
    ADD CONSTRAINT fk_req_act_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: resource_uris fk_resource_server_uris; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.resource_uris
    ADD CONSTRAINT fk_resource_server_uris FOREIGN KEY (resource_id) REFERENCES public.resource_server_resource(id);


--
-- Name: role_attribute fk_role_attribute_id; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.role_attribute
    ADD CONSTRAINT fk_role_attribute_id FOREIGN KEY (role_id) REFERENCES public.keycloak_role(id);


--
-- Name: realm_supported_locales fk_supported_locales_realm; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm_supported_locales
    ADD CONSTRAINT fk_supported_locales_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id);


--
-- Name: user_federation_config fk_t13hpu1j94r2ebpekr39x5eu5; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_federation_config
    ADD CONSTRAINT fk_t13hpu1j94r2ebpekr39x5eu5 FOREIGN KEY (user_federation_provider_id) REFERENCES public.user_federation_provider(id);


--
-- Name: realm fk_traf444kk6qrkms7n56aiwq5y; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.realm
    ADD CONSTRAINT fk_traf444kk6qrkms7n56aiwq5y FOREIGN KEY (master_admin_client) REFERENCES public.client(id);


--
-- Name: user_group_membership fk_user_group_user; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.user_group_membership
    ADD CONSTRAINT fk_user_group_user FOREIGN KEY (user_id) REFERENCES public.user_entity(id);


--
-- Name: policy_config fkdc34197cf864c4e43; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.policy_config
    ADD CONSTRAINT fkdc34197cf864c4e43 FOREIGN KEY (policy_id) REFERENCES public.resource_server_policy(id);


--
-- Name: identity_provider_config fkdc4897cf864c4e43; Type: FK CONSTRAINT; Schema: public; Owner: keycloak
--

ALTER TABLE ONLY public.identity_provider_config
    ADD CONSTRAINT fkdc4897cf864c4e43 FOREIGN KEY (identity_provider_id) REFERENCES public.identity_provider(internal_id);


--
-- PostgreSQL database dump complete
--

