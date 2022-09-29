import React, {FormEvent, useState} from "react";
import {Page} from "../Page/Page";
import {createUsers} from "../../Api/apiClient";
import {Link} from "react-router-dom";
import "./CreateUser.scss";

type FormStatus = "READY" | "SUBMITTING" | "ERROR" | "FINISHED"

export function CreateUserForm(): JSX.Element {
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [email, setEmail] = useState("");
    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");
    const [profileImageUrl, setProFileImageUrl] = useState("");
    const [coverImageUrl, setCoverImageUrl] = useState("");
    const [status, setStatus] = useState<FormStatus>("READY");

    function submitForm(event: FormEvent) {
        event.preventDefault();
        setStatus("SUBMITTING");
        createUsers({firstName, 
                    lastName, 
                    email, 
                    userName, 
                    password, 
                    profileImageUrl, 
                    coverImageUrl
                    })
            .then(() => setStatus("FINISHED"))
            .catch(() => setStatus("ERROR"));
    }
    
    if (status === "FINISHED") {
        return <div>
            <p>Create User Submitted Successfully!</p>
            <Link to="/">Return to your feed?</Link>
        </div>
    }

    return (
        <form className="create-post-form" onSubmit={submitForm}>
            <label className="form-label">
                First Name
                <input className="form-input" value={firstName} onChange={event => setFirstName(event.target.value)}/>
            </label>

            <label className="form-label">
                Last Name
                <input className="form-input" value={lastName} onChange={event => setLastName(event.target.value)}/>
            </label>

            <label className="form-label">
                Email
                <input className="form-input" value={email} onChange={event => setEmail(event.target.value)}/>
            </label>

            <label className="form-label">
                User Name
                <input className="form-input" value={userName} onChange={event => setUserName(event.target.value)}/>
            </label>

            <label className="form-label">
                Password
                <input className="form-input" value={password} onChange={event => setPassword(event.target.value)}/>
            </label>

            <label className="form-label">
                ProfileImageUrl
                <input className="form-input" value={profileImageUrl} onChange={event => setProFileImageUrl(event.target.value)}/>
            </label>

            <label className="form-label">
                Cover ImageUrl
                <input className="form-input" value={coverImageUrl} onChange={event => setCoverImageUrl(event.target.value)}/>
            </label>

            <button className="submit-button" disabled={status === "SUBMITTING"} type="submit">Create User</button>
            {status === "ERROR" && <p>Something went wrong! Please try again.</p>}
        </form>
    );
}

export function CreateUser(): JSX.Element {
    return (
        <Page containerClassName="create-user-page">
            <h1 className="title">Create user</h1>
            <CreateUserForm/>
        </Page>
    );
}