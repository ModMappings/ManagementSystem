package org.modmappings.mmms.api.util;

public final class Constants {
    public static final String MOD_MAPPINGS_OFFICIAL_AUTH = "ModMappings Official auth";
    public static final String BEARER_AUTH_SCHEME = "bearer";
    public static final String JWT_BEARER_FORMAT = "JWT";
    public static final String OFFICIAL_AUTH_DESC = "The official OpenID connect authentication server for ModMappings.";
    public static final String OFFICIAL_AUTH_OPENID_CONFIG_URL = "https://testauth.minecraftforge.net/auth/realms/ModMappings/.well-known/openid-configuration";
    public static final String OFFICIAL_AUTH_AUTHORIZATION_URL = "https://testauth.minecraftforge.net/auth/realms/ModMappings/protocol/openid-connect/auth";
    public static final String OFFICIAL_AUTH_TOKEN_URL = "https://testauth.minecraftforge.net/auth/realms/ModMappings/protocol/openid-connect/token";
    public static final String SCOPE_ROLES_NAME = "roles";
    public static final String SCOPE_ROLE_DESC = "Gets the roles the user is part of.";
    public static final String MOD_MAPPINGS_DEV_AUTH = "ModMappings Local development auth";
    public static final String DEV_AUTH_DESC = "The local development OpenID connect authentication server for ModMappings.";
    public static final String DEV_AUTH_OPENID_CONFIG_URL = "http://localhost:8081/auth/realms/ModMappings/.well-known/openid-configuration";
    public static final String DEV_AUTH_AUTHORIZATION_URL = "http://localhost:8081/auth/realms/ModMappings/protocol/openid-connect/auth";
    public static final String DEV_AUTH_TOKEN_URL = "http://localhost:8081/auth/realms/ModMappings/protocol/openid-connect/token";
}
