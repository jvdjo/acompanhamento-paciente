import { createContext, useContext, useState, useEffect } from 'react';

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const token = localStorage.getItem('token');
        const userName = localStorage.getItem('userName');
        const userPicture = localStorage.getItem('userPicture');

        if (token && userName) {
            setUser({ name: userName, token, picture: userPicture });
        }
        setLoading(false);
    }, []);

    const login = (token, name, picture = null) => {
        localStorage.setItem('token', token);
        localStorage.setItem('userName', name);
        if (picture) {
            localStorage.setItem('userPicture', picture);
        }
        setUser({ name, token, picture });
    };

    const logout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('userName');
        localStorage.removeItem('userPicture');
        setUser(null);
    };

    const isAuthenticated = !!user;

    return (
        <AuthContext.Provider value={{ user, login, logout, isAuthenticated, loading }}>
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
}

