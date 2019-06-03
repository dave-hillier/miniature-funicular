import { createHash } from 'crypto';

//const clientId = process.env.REACT_APP_OATH_CLIENT_ID!;
//const oAuthOrigin = process.env.REACT_APP_OAUTH_ORIGIN!;
const clientId = 'GC9TXdQR5EOFuKMHq4cGibzAHiksWcaA';
const oAuthOrigin = "https://dhi.eu.auth0.com/"; // TODO: redirect?

const tryParseResponse = async (response: Response) => {
    try {
        return await response.clone().json(); // Clone so we can access text later if necessary
    } catch (err) {
        return await response.clone().text();
    }
}

type FormPayload = Record<string, string | File | Array<File>>

interface RequestOptions {
    body?: object;
    form?: FormPayload;
    method?: 'POST' | 'PUT';
    query?: Record<string, string>;
    headers?: Record<string, string>;
}

const getContentType = (body: object | undefined): Record<string, string> => {
    if (body) return { 'content-type': 'application/json' };
    return {};
};

const addToForm = (form: FormData, [key, value]: [string, string | File | File[]]) => {
    if (Array.isArray(value)) value.forEach(val => form.append(key, val));
    else form.append(key, value);
    return form;
}

const formatBody = (body: object | undefined, form: FormPayload | undefined) => {
    if (body) return JSON.stringify(body);
    if (form) return Object.entries(form).reduce(addToForm, new FormData());
    return;
}

const formatOptions = (options: RequestOptions = {}): RequestInit => ({
    ...options,
    headers: {
        ...options.headers,
        ...getContentType(options.body),
    },
    body: formatBody(options.body, options.form),
});

const makeRequest = async (url: string, options?: RequestOptions) => {
    const response = await fetch(
        formatUrl(url, options && options.query),
        formatOptions(options)
    );
    const responseType = response.headers.get('content-type') || '';

    if (response.ok)
        return responseType.startsWith('application/json') ? await response.json() : null;

    throw Object.assign(new Error(), { response, body: await tryParseResponse(response) });
}

export interface OAuthConfig {
    issuer: string;
    jwks_uri: string;
    authorization_endpoint: string;
    token_endpoint: string;
    userinfo_endpoint: string;
    end_session_endpoint: string;
    check_session_iframe: string;
    revocation_endpoint: string;
    introspection_endpoint: string;
    frontchannel_logout_supported: boolean;
    frontchannel_logout_session_supported: boolean;
    backchannel_logout_supported: boolean;
    backchannel_logout_session_supported: boolean;
    scopes_supported: string[];
    claims_supported: string[];
    grant_types_supported: string[];
    response_types_supported: string[];
    response_modes_supported: string[];
    token_endpoint_auth_methods_supported: string[];
    subject_types_supported: string[];
    id_token_signing_alg_values_supported: string[];
    code_challenge_methods_supported: string[];
}

const getOAuthConfig = async (origin: string): Promise<OAuthConfig> =>
    makeRequest(new URL('/.well-known/openid-configuration', origin).toString());

export interface Token {
    accessToken: string;
    /**
     * Seconds until the token is no longer valid
     */
    expiresIn: number;
    /**
     * This one is the JWT access token
     */
    idToken: string;
    /**
     * This can be used to gain an updated accessToken
     */
    refreshToken: string;
    tokenType: string;
}

const formatUrl = (url: string, query: Record<string, string> = {}) =>
    Object.entries(query).reduce((agg, [key, value]) => `${agg}${agg.includes('?') ? '&' : '?'}${key}=${value}`, url);

const base64UrlFrombase64 = (base64: string) => base64
    .replace(/=/g, '')
    .replace(/\+/g, '-')
    .replace(/\//g, '_');

const generateRandomString = (length: number) =>
    'x'.repeat(length).replace(/x/g, () => String.fromCharCode(Math.floor(Math.random() * 20) + 65))

export const generateLoginUrl = async (returnUrl: string) => {
    const state = generateRandomString(25);
    const codeVerifier = localStorage.getItem('codeVerifier')
        || generateRandomString(50);
    const codeChallenge = base64UrlFrombase64(
        createHash('sha256')
            .update(codeVerifier)
            .digest('base64')
    );

    localStorage.setItem('codeVerifier', codeVerifier);
    localStorage.setItem(state, returnUrl);

    return {
        loginUrl: formatUrl(
            (await getOAuthConfig(oAuthOrigin)).authorization_endpoint,
            {
                state,
                client_id: clientId,

                redirect_uri: (`https://${window.location.host}/login`),
                response_type: 'code',
                scope: 'openid offline_access profile create:property update:property update:properties',
                audience: 'https://localhost:5001/',
                code_challenge: codeChallenge,
                code_challenge_method: 'S256',
            }
        ),
    };
};

const getTokenEndpoint = async () => (await getOAuthConfig(oAuthOrigin)).token_endpoint;

const snakeToCamel = (obj: Record<string, string>): any =>
    Object.entries(obj)
        .reduce(
            (agg, [key, value]) =>
                Object.assign(agg, { [key.replace(/_(.)/, (_, char) => char.toUpperCase())]: value }),
            {}
        );

const createToken = async (endpoint: string, body: Record<string, string>) =>
    makeRequest(endpoint, { body, method: 'POST' });

export const getToken = async (code: string): Promise<Token> => {
    const codeVerifier = localStorage.getItem('codeVerifier') || '';
    const token = await createToken(
        await getTokenEndpoint(),
        {
            client_id: clientId,
            redirect_uri: `https://${window.location.host}/login`,
            grant_type: 'authorization_code',
            code_verifier: codeVerifier,
            code,
        }
    );

    localStorage.removeItem('codeVerifier');

    return snakeToCamel(token);
}

export const refreshAccessToken = async (refreshToken: string): Promise<Token> =>
    snakeToCamel(
        await createToken(
            await getTokenEndpoint(),
            {
                client_id: clientId,
                grant_type: 'refresh_token',
                refresh_token: refreshToken,
            }
        )
    );
