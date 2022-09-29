import React, {createContext, ReactNode, useState} from "react";

interface LoginContextType {
    isLoggedIn: boolean,
    isAdmin: boolean,
    logIn: (username: string, password: string) => void,
    logOut: () => void,
    username: string,
    password:string,
}

export const LoginContext = createContext<LoginContextType>({
    isLoggedIn: false,
    isAdmin: false,
    logIn: (username: string, password: string) => {},
    logOut: () => {},
    username: "",
    password: "",
});

interface LoginManagerProps {
    children: ReactNode
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(false);
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    
    function logIn(username: string, password: string) {
        setLoggedIn(true);
        setUsername(username);
        setPassword(password);
    }
    
    function logOut() {
        setLoggedIn(false);
        setUsername("");
        setPassword("");
    }
    
    const context = {
        isLoggedIn: loggedIn,
        isAdmin: loggedIn,
        logIn: logIn,
        logOut: logOut,
        username: username,
        password:password,
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}