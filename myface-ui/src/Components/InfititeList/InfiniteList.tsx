import React, {ReactNode, useContext, useEffect, useState} from "react";
import {ListResponse} from "../../Api/apiClient";
import {Grid} from "../Grid/Grid";
import { LoginContext } from "../LoginManager/LoginManager";
import "./InfiniteList.scss";

interface InfiniteListProps<T> {
    fetchItems: (page: number, pageSize: number, username: string, password: string) => Promise<ListResponse<T>|null>;
    renderItem: (item: T) => ReactNode;
    
}

export function InfiniteList<T>(props: InfiniteListProps<T>): JSX.Element {
    const [items, setItems] = useState<T[]|null>([]);
    const [page, setPage] = useState(1);
    const [hasNextPage, setHasNextPage] = useState(false);
    const {username, password} = useContext(LoginContext);

    function replaceItems(response: ListResponse<T>|null) {
        setItems(response?.items ?? null);
        setPage(response?.page ?? 0);
        setHasNextPage((response?.nextPage ?? null) !== null);
    }

    function appendItems(response: ListResponse<T>|null) {
        setItems(items?.concat(response?.items ?? [])?? null);
        setPage(response?.page ?? 0);
        setHasNextPage((response?.nextPage ?? null) !== null);
    }
    
    useEffect(() => {
        props.fetchItems(1, 10, username, password)
            .then(replaceItems);
    }, [props]);

    function incrementPage() {
        props.fetchItems(page + 1, 10, username, password)
            .then(appendItems);
    }
    
    return (
        items !== null
            ? 
                <div className="infinite-list">
                    <Grid>
                        {items.map(props.renderItem)}
                    </Grid>
                    {hasNextPage && <button className="load-more" onClick={incrementPage}>Load More</button>}
                </div>
            :
                <div>
                    Error loading image
                </div>
    );
}