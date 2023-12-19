import { handler as getTaskDefinitionsHandler } from './../lambdas/task-definitions/index.js'

export const router = (app) => {
    app.get("/task-definitions", async (req, res) => {
        const result = convertResponse(
            await getTaskDefinitionsHandler(
                generateRequest(req),
                generateContext(req)
            )
        );
        sendResult(res, result);
    });
    app.get("/tasks", async (req, res) => {
        const result = convertResponse(
            await getTaskDefinitionsHandler(
                generateRequest(req),
                generateContext(req)
            )
        );
        sendResult(res, result);
    });
    app.put("/tasks/:id/status", async (req, res) => {
        const result = convertResponse(
            await getTaskDefinitionsHandler(
                generateRequest(req),
                generateContext(req)
            )
        );
        sendResult(res, result);
    });
    app.post("/task-definitions/:id/create", async (req, res) => {
        const result = convertResponse(
            await getTaskDefinitionsHandler(
                generateRequest(req),
                generateContext(req)
            )
        );
        sendResult(res, result);
    });
    app.post("/task-definitions/", async (req, res) => {
        const result = convertResponse(
            await getTaskDefinitionsHandler(
                generateRequest(req),
                generateContext(req)
            )
        );
        sendResult(res, result);
    })
};

function notFound() {
    return {
        statusCode: 404,
        body: ""
    };
}

function ok(body) {
    return {
        body: JSON.stringify(body),
        statusCode: 200,
    };
}

function sendResult(res, result) {
    console.log("Sending result", result);
    if (result.headers)
        for (let key of Object.keys(result.headers))
            res.set(key, result.headers[key]);
    res.status(result.statusCode);
    res.send(result.body);
}

function generateRequest(req) {
    return {
        body: JSON.stringify(req.body),
        headers: {},
        httpMethod: req.method,
        isBase64Encoded: false,
        multiValueHeaders: {},
        multiValueQueryStringParameters: {},
        path: req.url,
        pathParameters: req.params,
        queryStringParameters: req.query,
        requestContext: {
            accountId: "",
            apiId: "",
            authorizer: [],
            httpMethod: req.method,
            identity: {
                accessKey: "",
                accountId: "",
                apiKey: "",
                apiKeyId: "",
                caller: "",
                clientCert: {
                    clientCertPem: "",
                    issuerDN: "",
                    serialNumber: "",
                    subjectDN: "",
                    validity: {
                        notAfter: "",
                        notBefore: ""
                    }
                },
                cognitoAuthenticationProvider: "",
                cognitoAuthenticationType: "",
                cognitoIdentityId: "",
                cognitoIdentityPoolId: "",
                principalOrgId: "",
                sourceIp: "",
                user: "",
                userAgent: "",
                userArn: ""
            },
            path: req.url,
            protocol: "",
            requestId: "",
            requestTimeEpoch: 0,
            resourceId: "",
            resourcePath: "",
            stage: ""
        },
        resource: "",
        stageVariables: {}
    }
}

function generateContext(req) {
    return {
        awsRequestId: "",
        callbackWaitsForEmptyEventLoop: false,
        functionName: "",
        functionVersion: "",
        getRemainingTimeInMillis: () => 0,
        invokedFunctionArn: "",
        logGroupName: "",
        logStreamName: "",
        memoryLimitInMB: "",
        done: () => null,
        fail: () => null,
        succeed: () => null
    }
}

function convertResponse(res) {
    return res;
}