import Amplify, { Auth } from 'aws-amplify';

const _data = [];

const Authentication = {

    init: () => {
        Amplify.configure({
            Auth: {
                region: 'us-east-1',
                userPoolId: 'us-east-1_6u0RWKWaV',
                userPoolWebClientId: '3gqq1t3c01f55dd02srt13le9l'
            }
        });

        Auth.currentSession().then(session => _data.push(session))
    },

    signIn: (email, password) => Auth.signIn(email, password),
    signOut: () => Auth.signOut(),
    isAuthenticated: () => Auth.currentSession().then(() => true).catch(() => false),
    getToken: () => Auth.currentSession().then(session => session.idToken.jwtToken).catch(() => null)
}

Object.freeze(Authentication);
export default Authentication;